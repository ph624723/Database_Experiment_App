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
    public interface IProductionLineRepository : IRepository<ProductionLine>
    {

    }

    public class ProductionLineRepository : Repository<ProductionLine>, IProductionLineRepository
    {
        public ProductionLineRepository(OleDbConnection con): base(con)
        {
            
        }

        public override ProductionLine DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            List<int> processStreamsToAdd = new List<int>();
            string[] processStreamsStr = row["ProcessStreamIds"].ToString().Split(',');
            foreach (string id in processStreamsStr)
            {
                int bin;
                if (Int32.TryParse(id, out bin)) processStreamsToAdd.Add(bin);
            }

            return new ProductionLine()
            {
                Id = Int32.Parse(row["ProductionLineID"].ToString()),
                Name = row["ProductionLineName"].ToString(),
                Tpl = Int32.Parse(row["Tpl"].ToString()),
                Plant = Int32.Parse(row["Plant"].ToString()),
                ProcessStreamIds = processStreamsToAdd
            };
        }
        public override DataRow EntityToDataRow(ProductionLine entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            string processStreams = ",";
            foreach (int processStreamId in entity.ProcessStreamIds)
            {
                processStreams += processStreamId + ",";
            }

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["ProductionLineID"] = entity.Id;
            toReturn["ProductionLineName"] = entity.Name;
            toReturn["Tpl"] = entity.Tpl;
            toReturn["Plant"] = entity.Plant;
            toReturn["ProcessStreamIds"] = processStreams;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "ProductionLines";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProductionLineName", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Plant", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Tpl", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProcessStreamIds", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "ProductionLineID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("ProductionLineID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
