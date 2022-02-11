using Foto_DB.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foto_DB.Repositories
{
    public class TagRepository
    {
        public static void GetTags(string sqlConnectionString, ref List<Tags> tags)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            connection.Open();

            var query = "SELECT * FROM Tags";
            var command = new SqlCommand(query, connection);
            var cursor = command.ExecuteReader();
            while (cursor.Read())
            {
                var _id = cursor.GetInt32(0);
                var _name = cursor.GetString(1);
                var _pictureId = cursor.GetInt32(2);

                tags.Add(new Tags(_id, _name, _pictureId));
            }
            connection.Close();
        }

        public static void CreateTag(string sqlConnectionString, Tags tag)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            connection.Open();

            var query = $"INSERT INTO Tags (Id, TagName, PictureId) values ({tag.Id}, N'{tag.Name}', {tag.PictureId});";
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
