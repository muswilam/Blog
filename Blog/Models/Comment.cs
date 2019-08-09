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

        [Required , MaxLength(128)]
        public string Name { get; set; }

        [Required , DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime Time { get; set; }

        //nav props
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}