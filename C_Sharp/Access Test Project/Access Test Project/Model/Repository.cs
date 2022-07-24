using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access_Test_Project.Model
{
    /// <summary>
    /// Generic interface to work with access database tables.
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IRepository<Entity> where Entity:DatabaseObject
    {
        /// <summary>
        /// Returns whether the Repository has been changed during this session.
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        /// Updates the database with the changes made to the repository.
        /// </summary>
        void Commit();

        /// <summary>
        /// Adds the specified entity to the database.
        /// </summary>
        /// <param name="entity"></param>
        void Add(Entity entity);

        /// <summary>
        /// Adds the specified range of entities to the database.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(List<Entity> entities);

        /// <summary>
        /// Removes all entities matching the specified id from the database.
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        /// Removes all entities matching the specified entity's id from the database.
        /// </summary>
        /// <param name="entity"></param>
        void Remove(Entity entity);

        /// <summary>
        /// Removes all elements from the database.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the entity from the database which matches the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Entity Get(int id);

        /// <summary>
        /// Returns the first element from the database which matches the specified condition.
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        Entity FirstOrDefault(System.Func<Entity, bool> lambda);

        /// <summary>
        /// Returns all elements from the database.
        /// </summary>
        /// <returns></returns>
        ObservableCollection<Entity> GetAll();

        /// <summary>
        /// Returns all elements from the database whose ids are contained within the specified list.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ObservableCollection<Entity> Get(ICollection<int> ids);

        /// <summary>
        /// Returns all elements from the database which match the specified condition.
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        ObservableCollection<Entity> Where(System.Func<Entity, bool> lambda);

        /// <summary>
        /// Mapping Method: Returns an entity generated from the specified datarow.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        Entity DataRowToEntity(DataRow row);

        /// <summary>
        /// Mapping Method: Returns a datarow generated from the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rowToChange"></param>
        /// <returns></returns>
        DataRow EntityToDataRow(Entity entity, DataRow rowToChange);
    }

    public abstract class Repository<Entity> : IRepository<Entity> where Entity : DatabaseObject
    {
        protected readonly OleDbConnection _connection;
        protected OleDbDataAdapter _dataAdapter;
        private DataSet _ds;

        protected DataSet DS {
            get
            {
                if (!_initialized)
                {
                    _initialized = true;
                    Init();
                }
                return _ds;
            }
            set => _ds = value;
        }
        protected string _tableName;
        private bool _initialized;
        public bool HasChanges => _ds.HasChanges();

        public Repository(OleDbConnection connection)
        {
            _initialized = false;
            _connection = connection;
            _ds = new DataSet();
        }

        public void Add(Entity entity)
        {
            DataRow r;
            if (entity == null) return;
            if (entity.Id == 0)
            {
                entity.Id = FindFreeId();
                DS.Tables[_tableName].Rows.Add(EntityToDataRow(entity));
            }else if ((r = DS.Tables[_tableName].Rows.Find(entity.Id)) == null){
                DS.Tables[_tableName].Rows.Add(EntityToDataRow(entity));
            }
            else
            {
                r = EntityToDataRow(entity,r);
            }
        }

        public void AddRange(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                Add(entity);
            }
        }

        public void Remove(int id)
        {
            var tmp = DS.Tables[_tableName].Rows.Find(id);
            if (tmp != null)
            {
                tmp.Delete();
            }
        }

        public void Remove(Entity entity)
        {
            Remove(entity.Id);
        }

        public void Clear()
        {
            foreach (DataRow row in DS.Tables[_tableName].Rows) row.Delete();
        }

        public ObservableCollection<Entity> Where(System.Func<Entity,bool> lambda)
        {
            return new ObservableCollection<Entity>((from DataRow row in DS.Tables[_tableName].Rows select DataRowToEntity(row)).Where(lambda));
        }

        public Entity FirstOrDefault(System.Func<Entity, bool> lambda)
        {
            return (from DataRow row in DS.Tables[_tableName].Rows select DataRowToEntity(row)).Where(lambda).FirstOrDefault();
        }

        public Entity Get(int id)
        {
            return DataRowToEntity(DS.Tables[_tableName].Rows.Find(id));
        }

        public ObservableCollection<Entity> Get(ICollection<int> ids)
        {
            return Where(x => ids.Contains(x.Id));
        }

        public ObservableCollection<Entity> GetAll()
        {
            return new ObservableCollection<Entity>((from DataRow row in DS.Tables[_tableName].Rows select DataRowToEntity(row)).Where(x => x != null));
        }

        public void Commit()
        {
            if(_ds.HasChanges()) _dataAdapter.Update(DS, _tableName);
        }

        public abstract Entity DataRowToEntity(DataRow row);
        public abstract DataRow EntityToDataRow(Entity entity, DataRow rowToChange = null);
        public abstract void Init();

        protected OleDbCommand CreateTableCommand(string keyName, List<Tuple<string, OleDbType, int>> fieldnames)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = _connection;
            string commandText = "CREATE TABLE [" + _tableName + "]( [" + keyName + "] " + OleDbType.Integer + " PRIMARY KEY";
            fieldnames.ForEach(fieldname => commandText += ", [" + fieldname.Item1 + "] " + fieldname.Item2);
            commandText += ")";
            myCommand.CommandText = commandText;
            return myCommand;
        }

        private int FindFreeId()
        {
            int newId = 0;
            do{
                newId++;
            }while(DS.Tables[_tableName].Rows.Find(newId) != null);
            return newId;
        }

        protected bool TableExists()
            => _connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new string[] { null, null, _tableName }).Rows.Count != 0;


        protected bool ColumnExists(string columnName)
            => TableExists() ?
            _connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new string[] { null, null, _tableName }).AsEnumerable()
            .Any(row => ((string)row["COLUMN_NAME"]).Equals(columnName)) :
            false;
    }
}
