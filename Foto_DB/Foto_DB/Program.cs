using Foto_DB.Data;
using Foto_DB.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace Foto_DB
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=ASUS-DOSHAN;" +
                "Database=Gallery;" +
                "Trusted_Connection=True;";

            List<Picture> pictures = new List<Picture>();
            List<Tags> tags = new List<Tags>();

            Console.WriteLine("1 - Get all pictures, 2 - Create picture, 3 - Get all tags");
            int command = Convert.ToInt32(Console.ReadLine());

            switch (command)
            {
                case 1:
                    PictureRepository.GetPictures(connectionString, ref pictures);
                    foreach (Picture p in pictures)
                    {
                        Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Image path: {p.ImagePath}");
                        using (FileStream fstream = File.OpenRead($"{p.ImagePath}"))
                        {
                            byte[] array = new byte[fstream.Length];

                            fstream.Read(array, 0, array.Length);

                            string textFromFile = System.Text.Encoding.Default.GetString(array);
                            Console.WriteLine($"Текст из файла: \n{textFromFile}");
                        }
                    }
                    break;
                case 2:
                    Console.Write("Enter id: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter image path: ");
                    string imagePath = Console.ReadLine();

                    PictureRepository.CreatePicture(connectionString, new Picture(id, name, imagePath));
                    break;
                case 3:
                    TagRepository.GetTags(connectionString, ref tags);
                    foreach(Tags tag in tags)
                    {
                        Console.WriteLine($"Id: {tag.Id}, Name: {tag.Name}, PictureId: {tag.PictureId}");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
