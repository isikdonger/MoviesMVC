using APP.Domain;
using APP.Models;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services
{
    public class GenreService : DbService<Genre>, IService<GenreRequest, GenreResponse>
    {
        public GenreService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Genre> DbQuery()
        {
            return base.DbQuery()
                .Include(genreEntity => genreEntity.MovieGenres);
        }

        public CommandResponse Add(GenreRequest request)
        {
            if (DbQuery()
                .Any(genreEntity => 
                    genreEntity.Name == request.Name.Trim()))
                return Error("\"" + request.Name + "\" genre exists!");

            var genreEntity = new Genre
            {
                Name = request.Name.Trim()
            };
            DbAdd(genreEntity);

            return Success(genreEntity.Id, "\"" + request.Name + "\" genre created successfully!");
        }

        public GenreRequest Edit(int id)
        {
            var genreEntity = DbSingle(id);

            if (genreEntity is null)
                return null;

            return new GenreRequest()
            {
                Id = genreEntity.Id,
                Name = genreEntity.Name
            };
        }

        public IQueryable<GenreResponse> Query()
        {
            return DbQuery().Select(genreEntitiy => new GenreResponse()
            {
                Id = genreEntitiy.Id,
                Name = genreEntitiy.Name
            });
        }

        public CommandResponse Remove(int id)
        {
            var genreEntity = DbSingle(id);
            if (genreEntity is null)
                return Error("Genre not found!");

            DbRemove(genreEntity.MovieGenres);
            DbRemove(genreEntity);

            return Success(genreEntity.Id, "\"" + genreEntity.Name + "\" genre removed successfully!");
        }

        public CommandResponse Update(GenreRequest request)
        {
            if (DbQuery()
                .Any(genreEntity =>
                    genreEntity.Id != request.Id &&
                    genreEntity.Name == request.Name.Trim()))
                return Error("\"" + request.Name + "\" genre exists!");

            var genreEntity = DbSingle(request.Id);
            if (genreEntity is null)
                return Error("Genre not found!");

            genreEntity.Name = request.Name.Trim();
            DbUpdate(genreEntity);

            return Success(genreEntity.Id, "\"" + request.Name + "\" genre updated successfully!");
        }
    }
}
