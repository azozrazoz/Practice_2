using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterV1.Data;
using TwitterV1.UoW;

namespace TwitterV1.Repositories
{
    public class MessageEntityRepository
    {
        private readonly TwitterUnitOfWorkContext _context;
        public MessageEntityRepository(TwitterUnitOfWorkContext context)
        {
            _context = context;
        }


        public MessageEntity[] GetMessages() 
        {
            throw new NotImplementedException();
        }

        public MessageEntity[] GetMessagesByAuthorId(int authorId)
        {
            throw new NotImplementedException();
        }

        public MessageEntity[] GetMessagesByCreatedOnRange(
            DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public MessageEntity CreateMessage(
            MessageEntity messageToCreate)
        {
            throw new NotImplementedException();
        }

        public MessageEntity UpdateMessage(
            Guid id, MessageEntity messageToUpdate)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMessageById(Guid messageId)
        {
            var command = _context.CreateSqlCommand();

            var deleteMessageQuery = $"DELETE FROM [Messages] WHERE [Id] = '{messageId}'";
            command.CommandText = deleteMessageQuery;

            var affectedRowsCount = command.ExecuteNonQuery();
            return affectedRowsCount > 0;
        }
    }
}
