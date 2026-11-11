using CORE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Models
{
    public class GenreRequest : Record
    {
        [Required, StringLength(25)]
        public string Name { get; set; }
    }
}
