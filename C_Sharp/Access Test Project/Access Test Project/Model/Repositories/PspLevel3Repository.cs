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
    public interface IPspLevel3Repository : IRepository<PspLevel3>
    {

    }

    public class PspLevel3Repository : Repository<PspLevel3>, IPspLevel3Repository
    {
        public PspLevel3Repository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override PspLevel3 DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new PspLevel3()
            {
                Id = Int32.Parse(row["PspLevel3ID"].ToString()),
                Name = row["PspLevel3Name"].ToString(),
                PspLevel2 = Int32.Parse(row["PspLevel2"].ToString()),
            };
        }

        public override DataRow EntityToDataRow(PspLevel3 entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PspLevel3ID"] = entity.Id;
            toReturn["PspLevel3Name"] = entity.Name;
            toReturn["PspLevel2"] = entity.PspLevel2;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "PspLevel3s";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel3Name", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel2", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "PspLevel3ID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("PspLevel3ID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
