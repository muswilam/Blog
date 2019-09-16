using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Models;
using PagedList.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel
{
    public class PostViewModel
    {
        public Post Post { get; set; }

        public PagedList.IPagedList<Post> Posts { get; set; }

        public PagedList.IPagedList<Comment> Comments { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public string TagName { get; set; }

        public bool IsAdmin { get; set; }

        public int PageNumber { get; set; }
    }

    public class AddPostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, Enter post title."), MaxLength(225)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string PostImageUrl { get; set; }

        public HttpPostedFileBase PostImageFile { get; set; }

        [Required(ErrorMessage = "Please, Enter post body.")]
        public string Body { get; set; }

        //nav props
        public ICollection<Comment> Comments { get; set; }
    }
}