using System;

namespace TwitterV1.Data
{
    public class CommentEntity
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? AuthorId { get; set; }
        public Guid MessageId { get; set; }
    }
}
