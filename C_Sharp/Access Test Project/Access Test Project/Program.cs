using Access_Test_Project.Model;
using Access_Test_Project.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access_Test_Project
{
    class Program
    {
        public static void Main(String[] args)
        {
            using (var unit = new DataService(AppDomain.CurrentDomain.BaseDirectory + "\\test1.mdb"))//"C:\\Users\\Public\\test1.mdb"))
            {
                //MessageBox.Show(unit.Departments.GetAll().First().Name);
            }   
        }
    }
}
