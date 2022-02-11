using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foto_DB.Data
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public Picture(int id, string name, string imagePath)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath;
        }
    }
}
