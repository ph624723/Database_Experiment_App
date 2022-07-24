using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class ProcurementPosition : DatabaseObject
    {
        public ProcurementPosition()
        {
            CanEditRoleIds = new List<int>();
            PositionIds = new List<int>();
        }

        public string Topic { get; set; }
        public string TauNumber { get; set; }
        public List<int> CanEditRoleIds { get; set; }
        public double PlanTarget { get; set; }
        public double VowTarget { get; set; }
        public double CurrentTarget { get; set; }
        public List<int> PositionIds { get; set; }
        public int PspLevel4 { get; set; }
        public string ProcurementStatus { get; set; }

        public override string ToShow => Topic ?? "";
    }
}
