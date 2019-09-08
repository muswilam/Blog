using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Models;
using PagedList.Mvc;

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
    }
}