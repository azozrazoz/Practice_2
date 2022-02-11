using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterV1.Repositories;

namespace TwitterV1.UoW
{
    public class TwitterUnitOfWorkContext
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        public TwitterUnitOfWorkContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public SqlCommand CreateSqlCommand()
        {
            var command = new SqlCommand 
                { Connection = _connection, Transaction = _transaction };

            return command;
        }

        public bool SaveChanges()
        {
            try
            {
                _transaction.Commit();
                return true;
            }
            catch(Exception)
            {
                _transaction.Rollback();
                return false;
            }
        }
    }

    public class TwitterUnitOfWork
    {
        public MessageEntityRepository MessageEntityRepository { get; private set; }
        public CommentEntityRepository CommentEntityRepository { get; private set; }

        private readonly string _connectionString;
        private readonly TwitterUnitOfWorkContext _context;
        public TwitterUnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            _context = new TwitterUnitOfWorkContext(connectionString);

            MessageEntityRepository = new MessageEntityRepository(_context);
            CommentEntityRepository = new CommentEntityRepository(_context);
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
