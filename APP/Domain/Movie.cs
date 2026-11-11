using CORE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Domain
{
    public class Movie : Record
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; } // foreign key property

        public Director Director { get; set; } // navigation property

        public List<MovieGenre> MovieGenres { get; set; } = new(); // navigation property

        [NotMapped]
        public List<int> GenreIds
        {
            get => MovieGenres.Select(movieGenre => movieGenre.GenreId).ToList();
            set => MovieGenres = value.Select(genreId => new MovieGenre { GenreId = genreId }).ToList();
        }
    }
}
