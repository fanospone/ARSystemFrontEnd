using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ARSystem.Domain.DAL
{
    public class DbContext
    {
        private readonly IDbConnection _connection;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly LinkedList<UnitOfWork> _uows = new LinkedList<UnitOfWork>();

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connectionFactory">Connection</param>
        public DbContext(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.Create();
        }

        /// <summary>
        /// Create Unit of Work
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();
            var uow = new UnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _rwLock.EnterWriteLock();
            _uows.AddLast(uow);
            _rwLock.ExitWriteLock();

            return uow;
        }

        /// <summary>
        /// Create Command
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            var cmd = _connection.CreateCommand();

            _rwLock.EnterReadLock();
            if (_uows.Count > 0)
                cmd.Transaction = _uows.First.Value.Transaction;
            _rwLock.ExitReadLock();

            return cmd;
        }

        /// <summary>
        /// Remove Unit of Work
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveTransaction(UnitOfWork obj)
        {
            _rwLock.EnterWriteLock();
            _uows.Remove(obj);
            _rwLock.ExitWriteLock();
        }

        /// <summary>
        /// Dispose Connection
        /// </summary>
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
