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
    public interface IPlantRepository : IRepository<Plant>
    {

    }

    public class PlantRepository : Repository<Plant>, IPlantRepository
    {
        public PlantRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Plant DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Plant()
            {
                Id = Int32.Parse(row["PlantID"].ToString()),
                Name = row["PlantName"].ToString(),
                Shorthand = row["Shorthand"].ToString(),
            };
        }

        public override DataRow EntityToDataRow(Plant entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PlantID"] = entity.Id;
            toReturn["PlantName"] = entity.Name;
            toReturn["Shorthand"] = entity.Shorthand;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Plants";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("PlantName", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Shorthand", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "PlantID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("PlantID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
