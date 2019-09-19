using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Administrator
    {
        public int Id { get; set; }

        [Required , MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(128),]
        public string UserName { get; set; }

        [Required,RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") , MaxLength(128)]
        public string Email { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }

        [MaxLength(256)]
        public string Headline { get; set; }

        public DateTime? Birthdate { get; set; }

        [MaxLength(200)]
        public string Education { get; set; }

        [MaxLength(20)]
        public string Country { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [MaxLength(300)]
        public string ProfilePic { get; set; }

        //nav props 
        public ICollection<Skill> Skills { get; set; }
    }
}