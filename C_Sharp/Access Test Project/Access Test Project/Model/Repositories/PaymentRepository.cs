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
    public interface IPaymentRepository : IRepository<Payment>
    {

    }

    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(OleDbConnection con)
            : base(con)
        {
            
        }

        public override Payment DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            return new Payment()
            {
                Id = Int32.Parse(row["PaymentID"].ToString()),
                Amount = Double.Parse(row["Amount"].ToString()),
                Date = DateTime.Parse(row["PaymentDate"].ToString()),
                History = row["History"].ToString()
            };
        }

        public override DataRow EntityToDataRow(Payment entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["PaymentID"] = entity.Id;
            toReturn["Amount"] = entity.Amount;
            toReturn["PaymentDate"] = entity.Date;
            toReturn["History"] = entity.History;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "Payments";
            string keyName = "PaymentID";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("Amount", OleDbType.Double, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PaymentDate", OleDbType.Date, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("History", OleDbType.VarChar, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, keyName, fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand(keyName, fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
