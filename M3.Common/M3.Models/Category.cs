using System.Collections.Generic;

namespace M3.Models
{
    public class Category
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Name { get; set; }

        public List<Photo> Photos { get; set; }
    }
}