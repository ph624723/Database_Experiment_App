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
    public interface IProcurementPositionRepository : IRepository<ProcurementPosition>
    {

    }

    public class ProcurementPositionRepository : Repository<ProcurementPosition>, IProcurementPositionRepository
    {
        public ProcurementPositionRepository(OleDbConnection con): base(con)
        {
            
        }

        public override ProcurementPosition DataRowToEntity(DataRow row)
        {
            if (row == null || row.RowState.Equals(DataRowState.Deleted)) return null;

            List<int> rolesToAdd = new List<int>();
            string[] rolesStr = row["CanEditRoleIds"].ToString().Split(',');
            foreach (string id in rolesStr)
            {
                int bin;
                if (Int32.TryParse(id, out bin)) rolesToAdd.Add(bin);
            }

            List<int> positionsToAdd = new List<int>();
            string[] positionsStr = row["PositionIds"].ToString().Split(',');
            foreach (string id in positionsStr)
            {
                int bin;
                if (Int32.TryParse(id, out bin)) positionsToAdd.Add(bin);
            }

            return new ProcurementPosition()
            {
                Id = Int32.Parse(row["ProcurementPositionID"].ToString()),
                Topic = row["Topic"].ToString(),
                TauNumber = row["TauNumber"].ToString(),
                ProcurementStatus = row["ProcurementStatus"].ToString(),
                CanEditRoleIds = rolesToAdd,
                PositionIds = positionsToAdd,
                PspLevel4 = Int32.Parse(row["PspLevel4"].ToString()),
                PlanTarget = Double.Parse(row["PlanTarget"].ToString()),
                VowTarget = Double.Parse(row["VowTarget"].ToString()),
                CurrentTarget = Double.Parse(row["CurrentTarget"].ToString()),
            };
        }
        public override DataRow EntityToDataRow(ProcurementPosition entity, DataRow rowToChange = null)
        {
            if (entity == null) return null;

            string roles = ",";
            foreach (int roleId in entity.CanEditRoleIds)
            {
                roles += roleId + ",";
            }

            string positions = ",";
            foreach (int positionId in entity.PositionIds)
            {
                positions += positionId + ",";
            }

            DataRow toReturn = rowToChange == null || rowToChange.RowState.Equals(DataRowState.Deleted) ? DS.Tables[_tableName].NewRow() : rowToChange;
            toReturn["ProcurementPositionID"] = entity.Id;
            toReturn["Topic"] = entity.Topic;
            toReturn["TauNumber"] = entity.TauNumber;
            toReturn["ProcurementStatus"] = entity.ProcurementStatus;
            toReturn["CanEditRoleIds"] = roles;
            toReturn["PositionIds"] = positions;
            toReturn["PspLevel4"] = entity.PspLevel4;
            toReturn["PlanTarget"] = entity.PlanTarget;
            toReturn["VowTarget"] = entity.VowTarget;
            toReturn["CurrentTarget"] = entity.CurrentTarget;
            return toReturn;
        }

        public override void Init()
        {
            _tableName = "ProcurementPositions";
            List<Tuple<string, OleDbType, int>> fieldnames = new List<Tuple<string, OleDbType, int>>();
            fieldnames.Add(new Tuple<string, OleDbType, int>("Topic", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("TauNumber", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("ProcurementStatus", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("CanEditRoleIds", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PositionIds", OleDbType.VarChar, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PspLevel4", OleDbType.Integer, 5));
            fieldnames.Add(new Tuple<string, OleDbType, int>("PlanTarget", OleDbType.Double, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("VowTarget", OleDbType.Double, 40));
            fieldnames.Add(new Tuple<string, OleDbType, int>("CurrentTarget", OleDbType.Double, 40));
            _dataAdapter = DataAdapters.NewAdapter(_connection, _tableName, "ProcurementPositionID", fieldnames);
            try
            {
                if (!TableExists()) CreateTableCommand("ProcurementPositionID", fieldnames).ExecuteNonQuery();
                _dataAdapter.Fill(DS, _tableName);
            }
            catch (Exception e)
            {
                throw new Exception("Repository could not be created, watch inner exception for further information.", e);
            }
        }
    }
}
