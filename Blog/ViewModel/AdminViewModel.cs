using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.ViewModel
{
    public class AdminViewModel
    {
        [Required(ErrorMessage = "User Name's Required")]
        public string UserName { get; set; }

        public string Hash { get; set; }

        public string AdminErrorMsg { get; set; }

        public string PassErrorMsg { get; set; }
    }

    public class AboutAdminViewModel
    {
        public Administrator Administrator { get; set; }

        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress), MaxLength(128)]
        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }

        [MaxLength(200)]
        public string Education { get; set; }

        [MaxLength(20)]
        public string Country { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }
    }
}