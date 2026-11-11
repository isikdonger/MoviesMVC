using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CORE.Domain;

namespace APP.Domain
{
    public class Director : Record
    {
        [Required, StringLength(25)]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }

        public List<Movie> Movies { get; set; } = new(); // navigation property
    }
}
