namespace L4
{
    internal interface IChangeTracker<TEntity>
    {
        TEntity[] GetEntitiesToCreate();
    }
}
