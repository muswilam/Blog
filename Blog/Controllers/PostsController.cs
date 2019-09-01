using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Text;
using System.Data.Entity;
using PagedList;
using System.ServiceModel.Syndication;
using System.Web.Services;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        BlogContext context = new BlogContext();
        private int pageSize = 4;
        private const int postsPerFeed = 25;

        public ActionResult Index(string tagName, int? page)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                var tagPosts = GetTagsByTagName(tagName, page);
                return View(tagPosts);
            }

            int currentPage = page ?? 1;

            var Posts = context.Posts.Include(p => p.Tags)
                .OrderBy(p => p.Time).ToList();

            ViewBag.IsAdmin = IsAdmin;
            return View(Posts.ToPagedList(currentPage, pageSize));
        }

        public ActionResult RSS()
        {
            IEnumerable<SyndicationItem> posts = context.Posts
                .Where(p => p.Time < DateTime.Now)
                .OrderByDescending(p => p.Time)
                .Take(postsPerFeed)
                .ToList()
                .Select(p => GetSyndicationItem(p));

            //response is the feed of items
            SyndicationFeed feed = new SyndicationFeed("Dev", "Dev Community", new Uri("https://dev.to/"), posts);
            Rss20FeedFormatter formattedFeed = new Rss20FeedFormatter(feed);

            return new FeedResult(formattedFeed);
        }

        private SyndicationItem GetSyndicationItem(Post post)
        {
            return new SyndicationItem(post.Title, post.Body, new Uri("http://localhost:65008/Posts/Details/" + post.Id));
        }

        public ActionResult Details(int id)
        {
            var post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        //get view of add & edit
        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (var tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }
            ViewBag.Tags = tagList.ToString();
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        //post add & edit
        [ValidateInput(false)]
        public ActionResult Update(Post formModel, string tags)
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
            if (formModel.Id == 0)
            {
                context.Posts.Add(post);
            }
            context.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
        }

        public ActionResult Delete(int id)
        {
            if (IsAdmin)
            {
                var post = GetPost(id);
                context.Posts.Remove(post);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteComment(int id)
        {
            var comment = context.Comments.Where(c => c.Id == id).First();

            if (IsAdmin)
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }
            return RedirectToAction("Details", new { id = comment.PostId });
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Comment(int id, Comment commentForm)
        {
            JsonResult json = new JsonResult();

            var post = GetPost(id);
            var comment = new Comment();

            comment.Name = commentForm.Name;
            comment.Email = commentForm.Email;
            comment.Body = commentForm.Body;
            comment.Post = post;
            comment.Time = DateTime.Now;

            context.Comments.Add(comment);
            bool result = context.SaveChanges() > 0;

            if (result)
            {
                return Json(new { success = true } , JsonRequestBehavior.AllowGet);
            }
            else
               return Json(new { success = false, message = "OOPS!" } , JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetComments(int id)
        {
            var comments = context.Posts.Where(p => p.Id == id).Select(p => p.Comments).ToList();

            return Json(new { comments = comments }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tags(string tagName, int? page)
        {
            var tagPosts = GetTagsByTagName(tagName, page);
            return View("Index", tagPosts);
        }

        //get all posts of specific tag by tagName with pagination 
        public IPagedList<Post> GetTagsByTagName(string tagName , int? page)
        {
            int currentPage = page ?? 1;
            var tag = GetTagFromDb(tagName);
            ViewBag.IsAdmin = IsAdmin;
            return tag.Posts.ToPagedList(currentPage, pageSize);
        }

        private Tag GetTagFromDb(string tagName)
        {
            Tag tag = context.Tags.Include(t => t.Posts).Where(t => t.Name == tagName).FirstOrDefault() ?? new Tag() { Name = tagName };
            return tag;
        }

        private Post GetPost(int? id)
        {
            return (id.HasValue && id != 0) ? context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Where(p => p.Id == id)
                .First() : new Post() { Id = -1 , Tags = new List<Tag>()};
        }

        public bool IsAdmin 
        {
            get 
            {
                return Session["IsAdmin"] != null && (bool)Session["IsAdmin"];
            } 
        }
    }
}
