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
    public class DirectorService : DbService<Director>, IService<DirectorRequest, DirectorResponse>
    {
        public DirectorService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Director> DbQuery()
        {
            return base.DbQuery()
                .Include(d => d.Movies)
                .OrderBy(d => d.FirstName);
        }

        public CommandResponse Add(DirectorRequest request)
        {
            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();

            if (DbQuery()
                .Any(d => d.FirstName == firstName &&
                    d.LastName == lastName))
                return Error("Director with the same first and last name already exists!");

            var directorEntity = new Director
            {
                FirstName = firstName,
                LastName = lastName,
                IsRetired = request.IsRetired
            };
            DbAdd(directorEntity);

            return Success(directorEntity.Id, "Director added successfully!");
        }

        public DirectorRequest Edit(int id)
        {
            var directorEntity = DbSingle(id);

            if (directorEntity is null)
                return null;

            return new DirectorRequest
            {
                Id = directorEntity.Id,
                FirstName = directorEntity.FirstName,
                LastName = directorEntity.LastName,
                IsRetired = directorEntity.IsRetired
            };
        }

        public IQueryable<DirectorResponse> Query()
        {
            return DbQuery().Select(d => new DirectorResponse
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                IsRetired = d.IsRetired,
                FullNameR = d.FirstName + " " + d.LastName,
                MoviesR = d.Movies.Select(m => new MovieResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    TotalRevenue = m.TotalRevenue,
                    DirectorId = m.DirectorId
                }).ToList()
            });
        }

        public CommandResponse Remove(int id)
        {
            var directorEntity = DbSingle(id);
            if (directorEntity is null)
                return Error("Director not found!");

            if (directorEntity.Movies.Any())
                return Error("Director cannot be deleted because they have associated movies!");

            DbRemove(directorEntity);

            return Success(directorEntity.Id, "Director removed successfully!");
        }

        public CommandResponse Update(DirectorRequest request)
        {
            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();

            if (DbQuery()
                .Any(d => d.Id != request.Id &&
                    d.FirstName == firstName &&
                    d.LastName == lastName))
                return Error("Director with the same first and last name already exists!");

            var directorEntity = DbSingle(request.Id);
            if (directorEntity is null)
                return Error("Director not found!");

            directorEntity.FirstName = firstName;
            directorEntity.LastName = lastName;
            directorEntity.IsRetired = request.IsRetired;
            DbUpdate(directorEntity);

            return Success(directorEntity.Id, "Director updated successfully!");
        }
    }
}
