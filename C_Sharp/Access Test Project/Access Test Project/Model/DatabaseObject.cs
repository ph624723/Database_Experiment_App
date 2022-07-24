using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model
{
    public abstract class DatabaseObject : Object
    {
        public int Id { get; set; }

        public override bool Equals(object o)
        {
            return (o as DatabaseObject)?.Id.Equals(this.Id) ?? false;
        }

        public DatabaseObject ToDatabaseObject()
        {
            return this;
        }

        public abstract string ToShow { get;}
    }
}
