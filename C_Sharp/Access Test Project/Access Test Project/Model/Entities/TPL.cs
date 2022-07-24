using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class Tpl : DatabaseObject
    {
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Mail { get; set; }
        public string Number { get; set; }
        public int Department { get; set; }
        public int AssignedUser { get; set; }

        public override string ToShow => (Surname ?? "") + ", " + (Firstname ?? "");
    }
}
