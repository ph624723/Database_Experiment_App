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
    public interface IRoleRepository : IRepository<Role>
    {

    }

    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Role DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Role()
            {
                Id = Int32.Parse(row["RoleID"].ToString()),
                Name = row["RoleName"].ToString(),
            };
        }

        public override DataRow EntityToDataRow(Role entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["RoleID"] = entity.Id;
            toReturn["RoleName"] = entity.Name;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Roles";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("RoleName", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "RoleID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("RoleID", fieldnames).ExecuteNonQuery();
                    
                _dataAdapter.Fill(DS, _tableName);
                if (!GetAll().Any(role => role.Name.Equals("Basis"))) Add(new Role
                {
                    Name = "Basis"
                });
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
