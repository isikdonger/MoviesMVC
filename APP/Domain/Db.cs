using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }
    }
}
