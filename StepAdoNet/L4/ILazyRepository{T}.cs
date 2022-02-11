namespace L4
{
    public interface ILazyRepository<TEntity>
    {
        void CreateEntity(TEntity entityToCreate);
        TEntity[] ReadEntities();
        void UpdateEntityByPrimaryKey(int primaryKey, TEntity entityToUpdate);
        void DeleteEntityByPrimaryKey(int primaryKey);    
    }
}
