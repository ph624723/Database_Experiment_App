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
    public interface IPspLevel4Repository : IRepository<PspLevel4>
    {

    }

    public class PspLevel4Repository : Repository<PspLevel4>, IPspLevel4Repository
    {
        public PspLevel4Repository(OleDbConnection con)
            : base(con)
        {

        }

        public override PspLevel4 DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new PspLevel4()
            {
                Id = Int32.Parse(row["PspLevel4ID"].ToString()),
                Name = row["PspLevel4Name"].ToString(),
                PspLevel3 = Int32.Parse(row["PspLevel3"].ToString()),
            };
        }

        public override DataRow EntityToDataRow(PspLevel4 entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PspLevel4ID"] = entity.Id;
            toReturn["PspLevel4Name"] = entity.Name;
            toReturn["PspLevel3"] = entity.PspLevel3;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "PspLevel4s";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel4Name", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel3", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "PspLevel4ID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("PspLevel4ID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
