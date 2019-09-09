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
using Blog.ViewModel;
using Blog.Common;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        BlogContext context = new BlogContext();
        PostViewModel postModel = new PostViewModel();

        private const int postsPerFeed = 25;

        public ActionResult Index(string tagName, int? page , string postSearch)
        {
            //list posts by tags 
            if (!string.IsNullOrEmpty(tagName))
            {
                postModel.Posts = GetTagPostsByTagName(tagName, page);
                postModel.TagName = tagName;

                return View(postModel);
            }

            int currentPage = page ?? 1;
            
            //search
            if(!string.IsNullOrWhiteSpace(postSearch))
            {
                postModel.Posts = context.Posts
                    .Include(p => p.Tags)
                    .Where(p => p.Title.StartsWith(postSearch))
                    .OrderBy(p => p.Title)
                    .ToPagedList(currentPage, PageSize.pagePosts);

                return View(postModel);
            }

            var Posts = context.Posts.Include(p => p.Tags)
                .OrderBy(p => p.Time).ToList();

            ViewBag.IsAdmin = IsAdmin;
            postModel.IsAdmin = IsAdmin;
            postModel.PageNumber = currentPage;

            postModel.Posts = Posts.ToPagedList(currentPage, PageSize.pagePosts);
            return View(postModel);
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

        public ActionResult Details(int id , int? page)
        {
            int currentPage = page ?? 1;

            var post = GetPost(id);
            postModel.Post = post;
            postModel.Comments = post.Comments.OrderByDescending(c => c.Time).ToPagedList(currentPage, PageSize.pageComments);

            ViewBag.IsAdmin = IsAdmin;

            return View(postModel);
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
        public ActionResult Update([Bind(Exclude = "Time,EditTime,Tags")] Post formModel, string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }

            if(!ModelState.IsValid)
            {
                Post emptyPost = new Post();
                return View("edit", emptyPost);
            }

            //edit
            Post post = GetPost(formModel.Id);
            post.Title = formModel.Title;
            post.EditTime = DateTime.Now;
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
                post.EditTime = null;
                post.Time = DateTime.Now;
                context.Posts.Add(post);
            }
            context.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
        }

        public ActionResult Delete(int id , int? page)
        {
            int currentPage = page != 0 ? page.Value : 1;
            if (IsAdmin)
            {
                var post = GetPost(id);
                context.Posts.Remove(post);
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { page = currentPage});
        }

        [HttpPost]
        public JsonResult DeleteComment(int id)
        {
            var comment = context.Comments.Where(c => c.Id == id).First();

            bool result = false;
            if (IsAdmin)
            {
                context.Comments.Remove(comment);
                result = context.SaveChanges() > 0;
            }

            if (result)
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "OPPS! Something went wrong." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Comment(int id, Comment commentForm)
        {
            JsonResult json = new JsonResult();

            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.Select(v => v.Errors).ToList();

                var nameError = errors[1].Select(e => e.ErrorMessage).FirstOrDefault();
                var emailError = errors[2].Select(e => e.ErrorMessage).FirstOrDefault();
                var commentError = errors[3].Select(e => e.ErrorMessage).FirstOrDefault();

                return Json(new { modelNotValid = true , nameError = nameError , emailError = emailError , commentError = commentError });
            }

            var post = GetPost(id);
            var comment = new Comment();

            comment.Name = commentForm.Name;
            comment.Email = commentForm.Email;
            comment.Body = commentForm.Body;
            comment.Post = post;
            comment.Time = DateTime.Now;
            comment.IsAdmin = commentForm.IsAdmin;

            context.Comments.Add(comment);
            bool result = context.SaveChanges() > 0;

            if (result)
            {
                return Json(new { success = true } , JsonRequestBehavior.AllowGet);
            }
            else
               return Json(new { success = false, message = "OOPS! Something went wrong." } , JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetComments(int id)
        {
            var comments = context.Posts.Where(p => p.Id == id).Select(p => p.Comments).ToList();

            return Json(new { comments = comments }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tags(string tagName, int? page)
        {
            postModel.Posts = GetTagPostsByTagName(tagName, page);
            postModel.TagName = tagName;
            return View("Index", postModel);
        }

        [ChildActionOnly]
        public PartialViewResult TrendingTags()
        {
            postModel.Tags = context.Tags.Where(t => t.Posts.Count() >= TotalTrendingTags.TrendingTagsNumber).Include(t => t.Posts).ToList();
            postModel.IsAdmin = IsAdmin;
            return PartialView("_NavBar",postModel);
        }

        //get all posts of specific tag by tagName with pagination 
        public IPagedList<Post> GetTagPostsByTagName(string tagName , int? page)
        {
            int currentPage = page ?? 1;
            var tag = GetTagFromDb(tagName);
            ViewBag.IsAdmin = IsAdmin;
            return tag.Posts.ToPagedList(currentPage, PageSize.pagePosts);
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
