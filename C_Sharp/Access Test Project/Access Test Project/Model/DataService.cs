using Access_Test_Project.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access_Test_Project.Model
{
    public class DataService : IDisposable
    {
        private static string _filePath;
        public OleDbConnection Connection { get; private set; }
        public bool ToRollBack { get; private set; }

        public IUserRepository Users { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public ITplRepository Tpls { get; private set; }
        public IPlantRepository Plants { get; private set; }
        public IProcessStreamRepository ProcessStreams { get; private set; }
        public IProductionLineRepository ProductionLines { get; private set; }
        public IPositionRepository Positions { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IPspLevel2Repository PspLevel2s { get; private set; }
        public IPspLevel3Repository PspLevel3s { get; private set; }
        public IPspLevel4Repository PspLevel4s { get; private set; }
        public ISubtechnologyRepository Subtechnologies { get; private set; }


        void Init()
        {
            if (_filePath == null) throw new Exception("Filepath not yet initialized. The Filepath has to be initialized before the DataService can be called this way.");

            ToRollBack = false;

            string connetionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + _filePath;
            Connection = new OleDbConnection(connetionString);
            try
            {
                Users = new UserRepository(Connection);
                Roles = new RoleRepository(Connection);
                Departments = new DepartmentRepository(Connection);
                Tpls = new TplRepository(Connection);
                Plants = new PlantRepository(Connection);
                ProcessStreams = new ProcessStreamRepository(Connection);
                ProductionLines = new ProductionLineRepository(Connection);
                Positions = new PositionRepository(Connection);
                Projects = new ProjectRepository(Connection);
                PspLevel2s = new PspLevel2Repository(Connection);
                PspLevel3s = new PspLevel3Repository(Connection);
                PspLevel4s = new PspLevel4Repository(Connection);
                Subtechnologies = new SubtechnologyRepository(Connection);
            }
            catch (Exception e)
            {
                throw e;
            }
            Connection.Open();
        }

        public DataService()
        {
            try
            {
                Init();
            }
            catch (Exception e)
            {
                throw new Exception("DateService could not be created.", e);
            }
        }

        public DataService(string path)
        {
            _filePath = path;
            try
            {
                Init();
            }
            catch (Exception e)
            {
                throw new Exception("DateService could not be created.", e);
            }
        }

        public void Rollback()
        {
            ToRollBack = true;
        }

        public void Dispose()
        {
            if (!ToRollBack)
            {
                Users.Commit();
                Roles.Commit();
                Departments.Commit();
                Tpls.Commit();
                Plants.Commit();
                ProcessStreams.Commit();
                ProductionLines.Commit();
                Positions.Commit();
                Projects.Commit();
                PspLevel2s.Commit();
                PspLevel3s.Commit();
                PspLevel4s.Commit();
                Subtechnologies.Commit();
            }
            Connection.Close();
        }
    }
}
