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
    public interface ISubtechnologyRepository : IRepository<Subtechnology>
    {

    }

    public class SubtechnologyRepository : Repository<Subtechnology>, ISubtechnologyRepository
    {
        public SubtechnologyRepository(OleDbConnection con)
            : base(con)
        {
           
        }

        public override Subtechnology DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Subtechnology()
            {
                Id = Int32.Parse(row["SubtechnologyID"].ToString()),
                Name = row["SubtechnologyName"].ToString(),
                ProcessStream = Int32.Parse(row["ProcessStream"].ToString()),
            };
        }

        public override DataRow EntityToDataRow(Subtechnology entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = (rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted)) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["SubtechnologyID"] = entity.Id;
            toReturn["SubtechnologyName"] = entity.Name;
            toReturn["ProcessStream"] = entity.ProcessStream;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Subtechnologys";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("SubtechnologyName", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProcessStream", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "SubtechnologyID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("SubtechnologyID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
