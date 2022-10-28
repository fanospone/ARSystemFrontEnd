using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ARSystem.Domain.DAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UnitOfWork" in both code and config file together.
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<UnitOfWork> _rolledBack;
        private readonly Action<UnitOfWork> _committed;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="transaction">Transaction</param>
        /// <param name="rolledBack">Action</param>
        /// <param name="committed">Action</param>
        public UnitOfWork(IDbTransaction transaction, Action<UnitOfWork> rolledBack, Action<UnitOfWork> committed)
        {
            Transaction = transaction;
            _transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
        }

        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// Rollback Transaction
        /// </summary>
        public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack(this);
            _transaction = null;
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call save changes twice.");

            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }
    }
}
