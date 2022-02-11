using System;
using System.Collections.Generic;
using System.Linq;

namespace L4
{
    public class GenericLazyRepository<TEntity> : 
        ILazyRepository<TEntity>, 
        IChangeTracker<TEntity>,
        ITableNameContainer
    {
        private readonly string _repositoryTableName;
        public string GetMappedTableName => _repositoryTableName;

        private readonly ICollection<TEntity> _createEntityCollection;
        private readonly Dictionary<int, TEntity> _updateEntityCollection;
        private readonly ICollection<int> _deleteEntityCollection;

        public GenericLazyRepository()
        {
            _repositoryTableName = typeof(TEntity).Name;

            _createEntityCollection = new List<TEntity>();
            _updateEntityCollection = new Dictionary<int,TEntity>();
            _deleteEntityCollection = new List<int>();
        }

        public void CreateEntity(TEntity entityToCreate)
        {
            _createEntityCollection.Add(entityToCreate);
        }

        public TEntity[] GetEntitiesToCreate()
        {
            return _createEntityCollection.ToArray();
        }

        public void DeleteEntityByPrimaryKey(int primaryKey)
        {
            _deleteEntityCollection.Remove(primaryKey);
        }

        public TEntity[] ReadEntities()
        {
            throw new NotImplementedException();
        }

        public void UpdateEntityByPrimaryKey(int primaryKey, TEntity entityToUpdate)
        {
            _updateEntityCollection[primaryKey] = entityToUpdate;
        }
    }
}
