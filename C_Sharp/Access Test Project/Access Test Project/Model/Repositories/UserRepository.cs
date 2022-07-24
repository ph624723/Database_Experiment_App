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
    public interface IUserRepository : IRepository<User>
    {

    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(OleDbConnection con): base(con)
        {
            
        }

        public override User DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            List<int> rolesToAdd = new List<int>();
            string[] rolesStr = row["RoleIds"].ToString().Split(',');
            foreach (string id in rolesStr)
            {
                int bin;
                if (Int32.TryParse(id, out bin)) rolesToAdd.Add(bin);
            }

            return new User()
            {
                Id = Int32.Parse(row["UserID"].ToString()),
                Name = row["UserName"].ToString(),
                Mail = row["Mail"].ToString(),
                Password = row["Password"].ToString(),
                RoleIds = rolesToAdd
            };
        }
        public override DataRow EntityToDataRow(User entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            string roles = ",";
            foreach (int roleId in entity.RoleIds)
            {
                roles += roleId + ",";
            }

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["UserID"] = entity.Id;
            toReturn["UserName"] = entity.Name;
            toReturn["Mail"] = entity.Mail;
            toReturn["Password"] = entity.Password;
            toReturn["RoleIds"] = roles;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Users";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("UserName", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Mail", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Password", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("RoleIds", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "UserID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("UserID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
