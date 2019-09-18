using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class AdminProfile
    {
        public int AdminProfileId { get; set; }

        [MaxLength(300)]
        public string ProfileUrl { get; set; }

        //nav prop
        public Administrator Administrator { get; set; }

        [ForeignKey("Administrator")]
        public int AdminId { get; set; }
    }
}