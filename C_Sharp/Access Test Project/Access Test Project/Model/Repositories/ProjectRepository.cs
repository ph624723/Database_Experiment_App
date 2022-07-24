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
    public interface IProjectRepository : IRepository<Project>
    {

    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Project DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Project()
            {
                Id = Int32.Parse(row["ProjectID"].ToString()),
                Name = row["ProjectName"].ToString(),
            };
        }

        public override DataRow EntityToDataRow(Project entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["ProjectID"] = entity.Id;
            toReturn["ProjectName"] = entity.Name;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Projects";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProjectName", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "ProjectID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("ProjectID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
