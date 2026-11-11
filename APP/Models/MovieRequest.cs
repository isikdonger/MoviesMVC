using CORE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Models
{
    public class MovieRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("Movie Name")]
        public string Name { get; set; }

        // ReleaseDate is optional (DateTime? in the entity), so no Required.
        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        // decimal changed to decimal? to be able to use [Required], like Score in
        // UserRequest. Revenue can't be negative.
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 10000000000, ErrorMessage = "{0} must be 0 or more!")]
        [DisplayName("Total Revenue")]
        public decimal? TotalRevenue { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Director")]
        public int? DirectorId { get; set; }

        [Required(ErrorMessage = "At least one genre is required!")]
        [DisplayName("Genres")]
        public List<int> GenreIds { get; set; }
    }
}
