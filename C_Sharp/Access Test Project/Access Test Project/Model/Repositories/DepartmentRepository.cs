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
    public interface IDepartmentRepository : IRepository<Department>
    {

    }

    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Department DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Department()
            {
                Id = Int32.Parse(row["DepartmentID"].ToString()),
                Name = row["DepartmentName"].ToString(),
            };
        }

        public override DataRow EntityToDataRow(Department entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = (rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted)) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["DepartmentID"] = entity.Id;
            toReturn["DepartmentName"] = entity.Name;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Departments";
            string keyName = "DepartmentID";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("DepartmentName", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, keyName, fieldnames);
            try
            {
                if(!TableExists()) CreateTableCommand(keyName, fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
