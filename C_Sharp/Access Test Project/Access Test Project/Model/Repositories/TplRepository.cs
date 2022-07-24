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
    public interface ITplRepository : IRepository<Tpl>
    {

    }

    public class TplRepository : Repository<Tpl>, ITplRepository
    {
        public TplRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Tpl DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Tpl()
            {
                Id = Int32.Parse(row["TplID"].ToString()),
                Surname = row["Surname"].ToString(),
                Firstname = row["Firstname"].ToString(),
                Mail = row["Mail"].ToString(),
                Number = row["Number"].ToString(),
                Department = Int32.Parse(row["Department"].ToString()),
                AssignedUser = Int32.Parse(row["AssignedUser"].ToString())
            };
        }

        public override DataRow EntityToDataRow(Tpl entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["TplID"] = entity.Id;
            toReturn["Surname"] = entity.Surname;
            toReturn["Firstname"] = entity.Firstname;
            toReturn["Mail"] = entity.Mail;
            toReturn["Number"] = entity.Number;
            toReturn["Department"] = entity.Department;
            toReturn["AssignedUser"] = entity.AssignedUser;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Tpls";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("Surname", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Firstname", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Mail", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Number", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Department", OleDbType.Integer, 5));
            fieldnames.Add(new Tuple<string, OleDbType, int>("AssignedUser", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "TplID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("TplID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
