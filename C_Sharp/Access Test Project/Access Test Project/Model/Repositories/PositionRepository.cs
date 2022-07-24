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
    public interface IPositionRepository : IRepository<Position>
    {

    }

    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(OleDbConnection con): base(con)
        {
            
        }

        public override Position DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            List<int> paymentsToAdd = new List<int>();
            string[] paymentsStr = row["PlannedPaymentIds"].ToString().Split(',');
            foreach (string id in paymentsStr)
            {
                int bin;
                if (Int32.TryParse(id, out bin)) paymentsToAdd.Add(bin);
            }

            return new Position()
            {
                Id = Int32.Parse(row["PositionID"].ToString()),
                Number = row["Number"].ToString(),
                Topic = row["Topic"].ToString(),
                Remarks = row["Remarks"].ToString(),
                PlannedPaymentIds = paymentsToAdd,
                ProductionLine = Int32.Parse(row["ProductionLine"].ToString()),
                ProcessStream = Int32.Parse(row["ProcessStream"].ToString()),
            };
        }
        public override DataRow EntityToDataRow(Position entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            string payments = ",";
            foreach (int paymentId in entity.PlannedPaymentIds)
            {
                payments += paymentId + ",";
            }

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PositionID"] = entity.Id;
            toReturn["Number"] = entity.Number;
            toReturn["Topic"] = entity.Topic;
            toReturn["Remarks"] = entity.Remarks;
            toReturn["PlannedPaymentIds"] = payments;
            toReturn["ProductionLine"] = entity.ProductionLine;
            toReturn["ProcessStream"] = entity.ProcessStream;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Positions";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("Number", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Topic", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("Remarks", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PlannedPaymentIds", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProductionLine", OleDbType.Integer, 5));
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProcessStream", OleDbType.Integer, 5));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "PositionID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("PositionID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
