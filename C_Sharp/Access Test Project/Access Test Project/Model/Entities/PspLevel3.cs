using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class PspLevel3 : DatabaseObject
    {
        public string Name { get; set; }
        public int PspLevel2 { get; set; }

        public override string ToShow => Name;
    }
}
