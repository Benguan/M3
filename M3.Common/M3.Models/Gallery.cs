using System.Collections.Generic;

namespace M3.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Category { get; set; }

        public List<Photo> Photos { get; set; }
    }
}