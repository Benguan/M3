namespace M3.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string NormalUrl { get; set; }
    }
}
