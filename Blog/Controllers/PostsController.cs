using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Text;
using System.Data.Entity;
using PagedList;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        BlogContext context = new BlogContext();

        public ActionResult Index(int? page)
        {
            int currentPage = page ?? 1;
            int pageSize = 4;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var Posts = context.Posts.Include(p => p.Tags)
                .OrderBy(p => p.Time).ToList();

            ViewBag.IsAdmin = IsAdmin;
            return View(Posts.ToPagedList(currentPage, pageSize));
        } 

        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (var tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }
            ViewBag.Tags = tagList.ToString();
            return View(post);
        }

        [ValidateInput(false)]
        public ActionResult Update(Post formModel , string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }

            //edit
            Post post = GetPost(formModel.Id);
            post.Title = formModel.Title;
            post.Time = DateTime.Now;
            post.Body = formModel.Body;
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
            if(formModel.Id == 0)
            {
                context.Posts.Add(post);
            }
            context.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
        }

        private Tag GetTagFromDb(string tagName)
        {
            Tag tag = context.Tags.Where(t => t.Name == tagName).FirstOrDefault() ?? new Tag() { Name = tagName };
            return tag;
        }

        private Post GetPost(int? id)
        {
            return (id.HasValue && id != 0) ? context.Posts.Include(p => p.Tags).Where(p => p.Id == id).First() : new Post() { Id = -1 , Tags = new List<Tag>()};
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
