using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.ViewModel
{
    //for login
    public class AdminViewModel
    {
        [Required(ErrorMessage = "User Name's Required")]
        public string UserName { get; set; }

        public string Hash { get; set; }

        public string AdminErrorMsg { get; set; }

        public string PassErrorMsg { get; set; }
    }
    
    //for editiing admin profile
    public class AboutAdminViewModel
    {
        public Administrator Administrator { get; set; }

        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"), MaxLength(128)]
        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }

        [MaxLength(200)]
        public string Education { get; set; }

        [MaxLength(20)]
        public string Country { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [MaxLength(256)]
        public string Headline { get; set; }

        public List<string> SkillsTypes { get; set; }

        public Skill Skill { get; set; }

        public bool IsAdmin { get; set; }

        public string ProfileImgUrl { get; set; }
    }
}