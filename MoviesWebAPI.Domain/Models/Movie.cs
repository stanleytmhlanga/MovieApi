using System.ComponentModel.DataAnnotations;

namespace MoviesWebAPI.Domain.Models
{
    public class Movie
    {
        public Movie()
        { }
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Type { get; set; }
    }
}
