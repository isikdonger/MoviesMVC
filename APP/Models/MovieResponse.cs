using CORE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Models
{
    public class MovieResponse : Record
    {
        [DisplayName("Movie Name")]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        public List<int> GenreIds { get; set; }

        [DisplayName("Release Date")]
        public string ReleaseDateR { get; set; }

        [DisplayName("Total Revenue")]
        public string TotalRevenueR { get; set; }

        [DisplayName("Director")]
        public string DirectorNameR { get; set; }

        [DisplayName("Genres")]
        public string GenreNamesR { get; set; }
    }
}
