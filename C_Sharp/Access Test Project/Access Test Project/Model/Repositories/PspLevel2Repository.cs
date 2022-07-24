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
    public interface IPspLevel2Repository : IRepository<PspLevel2>
    {

    }

    public class PspLevel2Repository : Repository<PspLevel2>, IPspLevel2Repository
    {
        public PspLevel2Repository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override PspLevel2 DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new PspLevel2()
            {
                Id = Int32.Parse(row["PspLevel2ID"].ToString()),
                Name = row["PspLevel2Name"].ToString(),
                Project = Int32.Parse(row["Project"].ToString()),
            };
        }

        public override DataRow EntityToDataRow(PspLevel2 entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PspLevel2ID"] = entity.Id;
            toReturn["PspLevel2Name"] = entity.Name;
            toReturn["Project"] = entity.Project;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "PspLevel2s";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel2Name", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Project", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "PspLevel2ID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("PspLevel2ID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
