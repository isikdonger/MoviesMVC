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
    public class DirectorRequest : Record
    {
        [Required, StringLength(25)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Is Retired?")]
        public bool IsRetired { get; set; }
    }
}
