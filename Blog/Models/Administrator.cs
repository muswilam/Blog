using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Administrator
    {
        public int Id { get; set; }

        [Required , MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }
    }
}