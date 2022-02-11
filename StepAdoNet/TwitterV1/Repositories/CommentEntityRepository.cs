using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterV1.Data;
using TwitterV1.UoW;

namespace TwitterV1.Repositories
{
    public class CommentEntityRepository
    {
        private readonly TwitterUnitOfWorkContext _context;
        public CommentEntityRepository(TwitterUnitOfWorkContext context)
        {
            _context = context;
        }
        public CommentEntity CreateComment(CommentEntity commentToCreate)
        {
            throw new NotImplementedException();
        }
        public CommentEntity[] GetCommentsByMessageId(Guid messageId) 
        {
            var comments = new List<CommentEntity>();

            var query = $"SELECT * FROM [Comments] WHERE [MessageId] = '{messageId}'";
            var command = _context.CreateSqlCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var commentEntity = new CommentEntity
                {
                    Id = reader.GetGuid(0),
                    Content = reader.GetString(1),
                    CreatedOn = reader.GetDateTime(2),
                    AuthorId = 0,
                    MessageId = reader.GetGuid(4)
                };

                comments.Add(commentEntity);
            }

            return comments.ToArray();
        }

        public bool DeleteCommentById(Guid commentId)
        {
            var query = $"DELETE FROM [Comments] WHERE [Id] = '{commentId}'";
            var command = _context.CreateSqlCommand();
            command.CommandText = query;

            return command.ExecuteNonQuery() > 0;
        }
    }
}
