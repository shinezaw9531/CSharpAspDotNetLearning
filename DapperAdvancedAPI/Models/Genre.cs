namespace DapperAdvancedAPI.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GenreVm
    {
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
    }
}
