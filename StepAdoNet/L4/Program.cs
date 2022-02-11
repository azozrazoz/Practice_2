using L4.Tables;
using System;

namespace L4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=ASUS-DOSHAN;" +
                "Database=TwitterAppV1;" +
                "Trusted_Connection=True";

            var uow = new UnitOfWork(connectionString);
            var messageRepository = uow.Messages;

            /*messageRepository.CreateEntity(
                new MessageEntity(4, "Какой приятный день!", DateTime.Now, 12));*/

            //messageRepository.CreateEntity(
            //    new MessageEntity(2, "Какой приятный день!", DateTime.Now, 12));

            //messageRepository.CreateEntity(
            //    new MessageEntity(3, "Какой приятный день!", DateTime.Now, 12));

            /*messageRepository.DeleteEntityByPrimaryKey(2);
            messageRepository.DeleteEntityByPrimaryKey(3);
            messageRepository.UpdateEntityByPrimaryKey(1, new MessageEntity
            {
                Content = "Завтра я пойду в кино!"
            });

            var commentRepo = uow.Comments;
            commentRepo.UpdateEntityByPrimaryKey(1, new CommentEntity
            {
                Content = "ahahahaha"
            });*/

            

            uow.SaveChanges();
        }
    }
}
