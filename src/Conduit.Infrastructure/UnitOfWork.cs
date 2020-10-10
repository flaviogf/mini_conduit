using System;
using System.Data;

namespace Conduit.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        public UnitOfWork(IDbConnection connection)
        {
            connection.Open();

            Connection = connection;

            Transaction = connection.BeginTransaction();
        }

        public IDbConnection Connection { get; }

        public IDbTransaction Transaction { get; }

        public void Commit()
        {
            Transaction.Commit();

            Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();

            Dispose();
        }

        public void Dispose()
        {
            Transaction.Dispose();

            Connection.Dispose();
        }
    }
}
