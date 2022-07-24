using Access_Test_Project.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Repositories
{
    public interface IProcessStreamRepository : IRepository<ProcessStream>
    {

    }

    public class ProcessStreamRepository : Repository<ProcessStream>, IProcessStreamRepository
    {
        public ProcessStreamRepository(OleDbConnection con)
            : base(con)
        {
           
        }

        public override ProcessStream DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new ProcessStream()
            {
                Id = Int32.Parse(row["ProcessStreamID"].ToString()),
                Name = row["ProcessStreamName"].ToString(),
            };
        }

        public override DataRow EntityToDataRow(ProcessStream entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["ProcessStreamID"] = entity.Id;
            toReturn["ProcessStreamName"] = entity.Name;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "ProcessStreams";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProcessStreamName", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "ProcessStreamID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("ProcessStreamID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
