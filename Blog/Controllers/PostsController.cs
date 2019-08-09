using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        BlogContext context = new BlogContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update(int? id, string title, string body, DateTime dateTime, string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }

            //edit
            Post post = GetPost(id);
            post.Title = title;
            post.Time = dateTime;
            post.Body = body;
            post.Tags.Clear();

            //to avoid null referenece exception
            tags = tags ?? string.Empty;

            //stringSplitOption : for removing empty array from splitting
            var tagNames = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tagName in tagNames)
            {
                post.Tags.Add(GetTagFromDb(tagName));
            }

            //Create
            if(!id.HasValue)
            {
                context.Posts.Add(post);
            }
            context.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
        }

        private Tag GetTagFromDb(string tagName)
        {
            return context.Tags.Where(t => t.Name == tagName).First() ?? new Tag() { Name = tagName };
        }

        private Post GetPost(int? id)
        {
            return id.HasValue ? context.Posts.Where(p => p.Id == id).First() : new Post() { Id = -1 };
        }

        //Fix that later.
        public bool IsAdmin 
        {
            get 
            {
                return true;
                //return Session["IsAdmin"] != null && (bool)Session["IsAdmin"];
            } 
        }
    }
}
