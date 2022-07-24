using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    /// <summary>
    /// Represents one EKW or BANF
    /// </summary>
    public class Position : DatabaseObject
    {
        public Position()
        {
            PlannedPaymentIds = new List<int>();
            Remarks = "";
        }

        public string Number { get; set; }
        public string Topic { get; set; }
        public string Remarks { get; set; }
        public int ProductionLine { get; set; }
        public int ProcessStream { get; set; }
        public ICollection<int> PlannedPaymentIds { get; set; }

        public override string ToShow => Number ?? "" + ": " + Topic ?? "";
    }
}
