using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("{{ lowerclass }}")]
    public class {{ classname }}
    {
        [Key]
        public int Id { get; set; }

        {{ properties }}
    }
}