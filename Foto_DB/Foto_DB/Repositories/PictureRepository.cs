using Foto_DB.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foto_DB.Repositories
{
    public class PictureRepository
    {
        public static void GetPictures(string sqlConnectionString, ref List<Picture> pictures)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            connection.Open();

            var query = "SELECT * FROM Picture";
            var command = new SqlCommand(query, connection);
            var cursor = command.ExecuteReader();
            while (cursor.Read())
            {
                var _id = cursor.GetInt32(0);
                var _name = cursor.GetString(1);
                var _image = cursor.GetString(2);

                pictures.Add(new Picture(_id, _name, _image));
            }
            connection.Close();
        }

        public static void CreatePicture(string sqlConnectionString, Picture picture)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            connection.Open();

            var query = $"INSERT INTO Picture (Id, _name, _image) values ({picture.Id}, N'{picture.Name}', N'{picture.ImagePath}');";
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
