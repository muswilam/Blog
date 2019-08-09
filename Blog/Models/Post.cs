using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required , MaxLength(225)]
        public string Title { get; set; }

        public DateTime Time { get; set; }

        [Required]
        public string Body { get; set; }

        //nav props
        public ICollection<Tag> Tags { get; set; }
    }
}