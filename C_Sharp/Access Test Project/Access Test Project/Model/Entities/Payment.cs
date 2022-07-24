using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class Payment : DatabaseObject
    {
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string History { get; set; }

        public override string ToShow => Amount + ": " + Date.ToShortDateString();
    }
}
