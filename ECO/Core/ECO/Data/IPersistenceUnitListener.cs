namespace ECO.Data
{
    public interface IPersistenceUnitListener
    {
        #region Methods

        void ContextPreCreate(IPersistenceUnit unit);

        void ContextPostCreate(IPersistenceUnit unit, IPersistenceContext context);

        #endregion
    }
}
