using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Domain
{
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    {
        // The connection string in the configuration file
        // (appsettings.json).
        const string CONNECTIONSTRING = "data source=Db.db";

        public Db CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new Db(optionsBuilder.Options);
        }

        // Seed the database with initial data.
        public void SeedDb()
        {
            var db = CreateDbContext(null);

            var movies = db.Movies.ToList();
            db.Movies.RemoveRange(movies);
            var genres = db.Genres.ToList();
            db.Genres.RemoveRange(genres);
            var directors = db.Directors.ToList();
            db.Directors.RemoveRange(directors);
            var movieGenres = db.MovieGenres.ToList();
            db.MovieGenres.RemoveRange(movieGenres);
            db.SaveChanges();

            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='MovieGenres'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Genres'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Movies'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Directors'");

            // Insert new data:
            // Directors:
            var director = new Director
            {
                FirstName = "Christopher",
                LastName = "Nolan",
                IsRetired = false
            };
            db.Directors.Add(director);

            director = new Director
            {
                FirstName = "Greta",
                LastName = "Gerwig",
                IsRetired = false
            };
            db.Directors.Add(director);

            director = new Director
            {
                FirstName = "Steven",
                LastName = "Spielberg",
                IsRetired = false
            };
            db.Directors.Add(director);

            director = new Director
            {
                FirstName = "Sofia",
                LastName = "Coppola",
                IsRetired = false
            };
            db.Directors.Add(director);

            db.SaveChanges();

            // Genres:
            var genre = new Genre { Name = "Action" };
            db.Genres.Add(genre);

            genre = new Genre { Name = "Drama" };
            db.Genres.Add(genre);

            genre = new Genre { Name = "Science Fiction" };
            db.Genres.Add(genre);

            genre = new Genre { Name = "Comedy" };
            db.Genres.Add(genre);

            genre = new Genre { Name = "Adventure" };
            db.Genres.Add(genre);

            db.SaveChanges();

            // Movies with genres:
            db.Movies.Add(new Movie
            {
                Name = "Inception",
                ReleaseDate = new DateTime(2010, 7, 16),
                TotalRevenue = 839700000,
                DirectorId = db.Directors.Single(
                    d => d.FirstName == "Christopher" &&
                         d.LastName == "Nolan").Id,
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Action").Id
                    },
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Science Fiction").Id
                    }
                }
            });

            db.Movies.Add(new Movie
            {
                Name = "Barbie",
                ReleaseDate = new DateTime(2023, 7, 21),
                TotalRevenue = 1446000000,
                DirectorId = db.Directors.Single(
                    d => d.FirstName == "Greta" &&
                         d.LastName == "Gerwig").Id,
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Comedy").Id
                    },
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Drama").Id
                    }
                }
            });

            db.Movies.Add(new Movie
            {
                Name = "Jurassic Park",
                ReleaseDate = new DateTime(1993, 6, 11),
                TotalRevenue = 1109000000,
                DirectorId = db.Directors.Single(
                    d => d.FirstName == "Steven" &&
                         d.LastName == "Spielberg").Id,
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Adventure").Id
                    },
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Science Fiction").Id
                    }
                }
            });

            db.Movies.Add(new Movie
            {
                Name = "Lost in Translation",
                ReleaseDate = new DateTime(2003, 10, 3),
                TotalRevenue = 118700000,
                DirectorId = db.Directors.Single(
                    d => d.FirstName == "Sofia" &&
                         d.LastName == "Coppola").Id,
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Drama").Id
                    },
                    new MovieGenre
                    {
                        GenreId = db.Genres.Single(
                            g => g.Name == "Comedy").Id
                    }
                }
            });

            db.SaveChanges();
        }
    }
}
