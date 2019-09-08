using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name's Required.") , MaxLength(128)]
        public string Name { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Comment is Required.")]
        public string Body { get; set; }

        public DateTime Time { get; set; }

        public bool IsAdmin { get; set; }

        //nav props
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}