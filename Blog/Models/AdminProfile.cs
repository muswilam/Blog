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
        [Key]
        [ForeignKey("Administrator")]
        public int AdminProfileId { get; set; }

        [MaxLength(300)]
        public string ProfileUrl { get; set; }

        //nav prop
        public virtual Administrator Administrator { get; set; }
    }
}