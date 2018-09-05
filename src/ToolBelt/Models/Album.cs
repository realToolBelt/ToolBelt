namespace ToolBelt.Models
{
    public class Album
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Photo
    {
        /// <summary>
        /// Gets or sets the id of the <see cref="Album" /> the photo belongs to. If <c>null</c>, the
        /// photo does not belong to an album.
        /// </summary>
        public int? AlbumId { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public byte[] Image { get; set; }

        public string Name { get; set; }
    }
}
