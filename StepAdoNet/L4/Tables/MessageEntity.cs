using System;

namespace L4.Tables
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? AuthorId { get; set; }

        public MessageEntity()
        {

        }
        public MessageEntity(int id, string content, DateTime createdOn, int? authorId)
        {
            Id = id;
            Content = content;
            CreatedOn = createdOn;
            AuthorId = authorId;
        }
    }
}
