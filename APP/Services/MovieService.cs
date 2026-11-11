using APP.Domain;
using APP.Models;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class MovieService : DbService<Movie>, IService<MovieRequest, MovieResponse>
    {
        public MovieService(DbContext db) : base(db)
        {
        }

        public CommandResponse Add(MovieRequest request)
        {
            var name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return Error("Movie name is required!");

            if (DbQuery().Any(movie => movie.Name == name))
                return Error("A movie with the same name already exists!");

            if (!request.DirectorId.HasValue)
                return Error("Director is required!");

            if (request.GenreIds is null || request.GenreIds.Count == 0)
                return Error("At least one genre is required!");

            var entity = new Movie
            {
                Name = name,
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue ?? 0,
                DirectorId = request.DirectorId.Value,
                MovieGenres = request.GenreIds
                    .Distinct()
                    .Select(genreId => new MovieGenre
                    {
                        GenreId = genreId
                    })
                    .ToList()
            };

            DbAdd(entity);

            return Success(entity.Id, "Movie created successfully.");
        }

        public MovieRequest Edit(int id)
        {
            var entity = DbSingle(id);

            if (entity is null)
                return null;

            return new MovieRequest
            {
                Id = entity.Id,
                Name = entity.Name,
                ReleaseDate = entity.ReleaseDate,
                TotalRevenue = entity.TotalRevenue,
                DirectorId = entity.DirectorId,
                GenreIds = entity.MovieGenres
                    .Select(movieGenre => movieGenre.GenreId)
                    .ToList()
            };
        }

        protected override IQueryable<Movie> DbQuery()
        {
            return base.DbQuery()
                .Include(movie => movie.Director)
                .Include(movie => movie.MovieGenres)
                    .ThenInclude(movieGenre => movieGenre.Genre)
                .OrderBy(movie => movie.Name);
        }

        public IQueryable<MovieResponse> Query()
        {
            return DbQuery().Select(movie => new MovieResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                ReleaseDate = movie.ReleaseDate,
                TotalRevenue = movie.TotalRevenue,
                DirectorId = movie.DirectorId,

                GenreIds = movie.MovieGenres
                    .Select(movieGenre => movieGenre.GenreId)
                    .ToList(),

                ReleaseDateR = movie.ReleaseDate.HasValue
                    ? movie.ReleaseDate.Value.ToString("MM/dd/yyyy")
                    : string.Empty,
                TotalRevenueR = movie.TotalRevenue.ToString("N2"),
                DirectorNameR = movie.Director.FirstName + " " + movie.Director.LastName,
                GenreNamesR = string.Join("<br>",
                    movie.MovieGenres.Select(movieGenre => movieGenre.Genre.Name))
            });
        }

        public CommandResponse Remove(int id)
        {
            var entity = DbSingle(id);

            if (entity is null)
                return Error("Movie not found!");

            DbRemove(entity.MovieGenres);
            DbRemove(entity);

            return Success(entity.Id, "Movie deleted successfully.");
        }

        public CommandResponse Update(MovieRequest request)
        {
            var name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return Error("Movie name is required!");

            if (DbQuery().Any(movie =>
                    movie.Id != request.Id &&
                    movie.Name == name))
            {
                return Error("A movie with the same name already exists!");
            }

            if (!request.DirectorId.HasValue)
                return Error("Director is required!");

            if (request.GenreIds is null || request.GenreIds.Count == 0)
                return Error("At least one genre is required!");

            var entity = DbSingle(request.Id);

            if (entity is null)
                return Error("Movie not found!");

            entity.Name = name;
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue ?? 0;
            entity.DirectorId = request.DirectorId.Value;

            DbRemove(entity.MovieGenres);

            entity.MovieGenres = request.GenreIds
                .Distinct()
                .Select(genreId => new MovieGenre
                {
                    MovieId = entity.Id,
                    GenreId = genreId
                })
                .ToList();

            DbUpdate(entity);

            return Success(entity.Id, "Movie updated successfully.");
        }
    }
}
