using System;

namespace ECO.Data
{
    public interface IDataTransaction : IDisposable
    {
        #region Properties

        IPersistenceContext Context { get; }

        #endregion

        #region Methods

        void Commit();

        void Rollback();

        #endregion
    }
}
