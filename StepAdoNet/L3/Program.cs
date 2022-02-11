using System;
using System.Data.SqlClient;

namespace L3
{
    #region Data
    public class MessageEntity
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? AuthorId { get; set; }
    }
    public class CommentEntity
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? AuthorId { get; set; }
        public Guid MessageId { get; set; }
    }

    #endregion

    #region Repositories
    public class MessageRepository
    {
        private readonly UnitOfWorkContext _unitOfWorkContext;
        public MessageRepository(
            UnitOfWorkContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }

        public int DeleteMessage(Guid messageId)
        {
            try
            {
                var query = $"DELETE FROM [Messages] WHERE Id = '{messageId}'";

                var queryCommand = _unitOfWorkContext.GetSqlCommandContainer();
                queryCommand.CommandText = query;
                return queryCommand.ExecuteNonQuery();
            }
            catch (Exception ex) { return -1; }     
        }
    }
    public class CommentRepository
    {
        private readonly UnitOfWorkContext _unitOfWorkContext;
        public CommentRepository(UnitOfWorkContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }

        public int DeleteComment(Guid commentId)
        {
            var query = $"DELETE FROM [Comments] WHERE Id = '{commentId}'";
            var queryCommand = _unitOfWorkContext.GetSqlCommandContainer();
            queryCommand.CommandText = query;

            return queryCommand.ExecuteNonQuery();
        }
    }
    #endregion

    #region Unit of Work
    public class UnitOfWorkContext
    {
        private readonly SqlConnection _sqlConnection;
        private readonly SqlTransaction _sqlTransaction;
        private bool _exceptionOccuredDuringTransactionScope;

        public UnitOfWorkContext(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlTransaction = _sqlConnection.BeginTransaction();
            _exceptionOccuredDuringTransactionScope = false;
        }

        public void SetExceptionOccuredDuringTransaction()
        {
            _exceptionOccuredDuringTransactionScope = true;
        }

        public SqlCommand GetSqlCommandContainer() 
        {
            var sqlCommandContainer = new SqlCommand() 
            { 
                Connection = _sqlConnection,
                Transaction = _sqlTransaction
            };

            return sqlCommandContainer;
        } 

        public void Commit()
        {
            try
            {
                if (_exceptionOccuredDuringTransactionScope)
                    throw new Exception("Exception occured during transaction scope");

                _sqlTransaction.Commit();
            }
            catch (Exception)
            {
                _sqlTransaction.Rollback();
            }
            finally
            {
                _sqlTransaction.Dispose();
            }
        }
    }
    public class UnitOfWork
    {
        private readonly SqlConnection _sqlConnection;
        private readonly UnitOfWorkContext _unitOfWorkContext;
        public MessageRepository MessageRepository { get; private set; }
        public CommentRepository CommentRepository { get; private set; }

        public UnitOfWork(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();

            _unitOfWorkContext = new UnitOfWorkContext(_sqlConnection);

            MessageRepository = new MessageRepository(_unitOfWorkContext);
            CommentRepository = new CommentRepository(_unitOfWorkContext);
        }

        public void SaveChanges()
        {
            _unitOfWorkContext.Commit();
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=(localdb)\\MSSQLLocalDB;" +
                "Database=TwitterAppDb1;" +
                "Trusted_Connection=True";

            // 16:07:28 - BT
            var unitOfWork = new UnitOfWork(connectionString);

            var messageRepository = unitOfWork.MessageRepository;
            var commentRepository = unitOfWork.CommentRepository;

            // do some calculation
            
            // 16:07:32
            commentRepository.DeleteComment(Guid.Parse("2b011151-77e7-4427-83ac-6ae69d7711c9")); 

            // 16:07:32
            commentRepository.DeleteComment(Guid.Parse("2b011151-77e7-4427-83ac-6ae69d7711c0"));

            // Do some calculation here...

            // 16:07:35
            messageRepository.DeleteMessage(Guid.Parse("2b011151-77e7-4427-83ac-6ae69d7711c8"));

            // 16:07:35 - ET (Commit) =>  16:07:35 - 16:07:28 <= 100 ms
            unitOfWork.SaveChanges();
        }
    }
}
