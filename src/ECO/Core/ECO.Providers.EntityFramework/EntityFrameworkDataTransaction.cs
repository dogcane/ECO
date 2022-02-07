﻿using ECO.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public sealed class EntityFrameworkDataTransaction : IDataTransaction
    {
        #region Fields

        private bool _disposed = false;

        #endregion

        #region IDataTransaction Members

        public IDbContextTransaction Transaction { get; private set; }

        public EntityFrameworkPersistenceContext Context { get; private set; }

        IPersistenceContext IDataTransaction.Context => Context;

        #endregion

        #region ~Ctor

        private EntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context, IDbContextTransaction transaction)
        {
            Transaction = transaction;
            Context = context;
        }

        ~EntityFrameworkDataTransaction()
        {
            Dispose(false);
        }

        #endregion

        #region Factory_Methods

        internal static EntityFrameworkDataTransaction CreateEntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context)
        {
            var transaction = context.Context.Database.BeginTransaction();
            return new EntityFrameworkDataTransaction(context, transaction);
        }

        internal static async Task<EntityFrameworkDataTransaction> CreateEntityFrameworkDataTransactionAsync(EntityFrameworkPersistenceContext context, CancellationToken cancellationToken = default)
        {
            var transaction = await context.Context.Database.BeginTransactionAsync();
            return new EntityFrameworkDataTransaction(context, transaction);
        }

        #endregion

        #region Methods

        public void Commit() => Transaction.Commit();

        public void Rollback() => Transaction.Rollback();

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await Transaction.CommitAsync(cancellationToken);        

        public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Transaction.RollbackAsync(cancellationToken);

        public void Dispose() => Dispose(true);

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }
                GC.SuppressFinalize(this);
            }
            _disposed = true;
            Transaction = null;
            Context = null;
        }

        #endregion
    }
}
