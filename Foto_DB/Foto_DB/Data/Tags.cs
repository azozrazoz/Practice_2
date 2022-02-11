using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foto_DB.Data
{
    public class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PictureId { get; set; }

        public Tags(int id, string name, int pictureId)
        {
            Id = id;
            Name = name;
            PictureId = pictureId;
        }
    }
}
