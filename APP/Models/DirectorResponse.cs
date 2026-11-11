using CORE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Models
{
    public class DirectorResponse : Record
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }

        // FirstName + " " + LastName concatenated value.
        [DisplayName("Full Name")]
        public string FullNameR { get; set; }

        [DisplayName("Movies")]
        public List<MovieResponse> MoviesR { get; set; }
    }
}
