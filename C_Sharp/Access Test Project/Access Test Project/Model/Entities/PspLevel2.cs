using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class PspLevel2 : DatabaseObject
    {
        public string Name { get; set; }
        public int Project { get; set; }

        public override string ToShow => Name;
    }
}
