﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, Enter post title.") , MaxLength(225)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string PostImageUrl { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? EditTime { get; set; }

        [Required(ErrorMessage = "Please, Enter post body.")]
        public string Body { get; set; }

        public int? PinCommentId { get; set; }

        //nav props
        public ICollection<Tag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}