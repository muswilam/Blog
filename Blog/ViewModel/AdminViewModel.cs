using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}