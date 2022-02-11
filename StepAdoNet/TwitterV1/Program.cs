using System;
using TwitterV1.Data;
using TwitterV1.UoW;

namespace TwitterV1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=(localdb)\\MSSQLLocalDB;" +
                "Database=TwitterAppV1;" +
                "Trusted_Connection=True";

            var twitterUoW = new TwitterUnitOfWork(connectionString);
            var commentRepository = twitterUoW.CommentEntityRepository;
            var messageRepository = twitterUoW.MessageEntityRepository;
            var messageId = Guid.Parse("A2EB706B-A939-4223-A984-82DFD273A7ED");
            var comments = commentRepository.GetCommentsByMessageId(messageId);

            foreach (var comment in comments)
            {
                commentRepository.DeleteCommentById(comment.Id);
            }

            messageRepository.DeleteMessageById(messageId);

            twitterUoW.Commit();
            

            Console.WriteLine("Hello World!");
        }
    }
}
