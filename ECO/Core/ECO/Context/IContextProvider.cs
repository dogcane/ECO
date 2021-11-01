namespace ECO.Context
{
    public interface IContextProvider
    {
        #region Methods

        object GetContextData(string dataKey);

        void SetContextData(string dataKey, object data);

        #endregion
    }
}
