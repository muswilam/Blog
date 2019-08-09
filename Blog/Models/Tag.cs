using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required , MaxLength(64)]
        public string Name { get; set; }

        //nav props
        public ICollection<Post> Posts { get; set; }
    }
}