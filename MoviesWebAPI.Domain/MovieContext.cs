using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using MoviesWebAPI.Domain.Models;

namespace MoviesWebAPI.Domain
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("MoviesDb")
        {}
        public DbSet<Movie> Movies { get; set; }

    }
}
