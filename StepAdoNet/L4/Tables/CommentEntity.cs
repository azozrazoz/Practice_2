using System;

namespace L4.Tables
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? AuthorId { get; set; }
        public int MessageId { get; set; }

        public CommentEntity()
        {

        }
        public CommentEntity(int id, string content, DateTime createdOn, int? authorId, int messageId)
        {
            Id = id;
            Content = content;
            CreatedOn = createdOn;
            AuthorId = authorId;
            MessageId = messageId;
        }
    }
}
