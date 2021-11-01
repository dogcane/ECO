namespace ECO.Data
{
    public static class EntityExtensions
    {
        public static PersistenceState GetPersistenceState<T, K>(this T entity) where T : IAggregateRoot<K>
        {
            return DataContext.Current.GetPersistenceState<T, K>(entity);
        }

        public static IPersistenceContext GetPersistenceContext<T, K>(this T entity) where T : IAggregateRoot<K>
        {
            return DataContext.Current.GetCurrentContext(entity);
        }
    }
}
