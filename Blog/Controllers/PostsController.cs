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
using System.IO;

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
                .Include(p => p.Comments)
                .OrderByDescending(p => p.Time).ToList();

            ViewBag.IsAdmin = IsAdmin;
            postModel.IsAdmin = IsAdmin;
            postModel.PageNumber = currentPage;

            postModel.Posts = Posts.ToPagedList(currentPage, PageSize.pagePosts);
            return View(postModel);
        }

        //public ActionResult RSS()
        //{
        //    IEnumerable<SyndicationItem> posts = context.Posts
        //        .Where(p => p.Time < DateTime.Now)
        //        .OrderByDescending(p => p.Time)
        //        .Take(postsPerFeed)
        //        .ToList()
        //        .Select(p => GetSyndicationItem(p));

        //    //response is the feed of items
        //    SyndicationFeed feed = new SyndicationFeed("Dev", "Dev Community", new Uri("https://dev.to/"), posts);
        //    Rss20FeedFormatter formattedFeed = new Rss20FeedFormatter(feed);

        //    return new FeedResult(formattedFeed);
        //}

        private SyndicationItem GetSyndicationItem(Post post)
        {
            return new SyndicationItem(post.Title, post.Body, new Uri("http://localhost:65008/Posts/Details/" + post.Id));
        }

        public ActionResult Details(int id , int? page)
        {
            int currentPage = page ?? 1;

            var post = GetPost(id);
            postModel.Post = post;

            if(post.PinCommentId.HasValue)
            {
                postModel.Comments = post.Comments.OrderByDescending(c => c.Id == post.PinCommentId.Value).ThenByDescending(c => c.Time).ToPagedList(currentPage, PageSize.pageComments);
            }
            else
            {
                postModel.Comments = post.Comments.OrderByDescending(c => c.Time).ToPagedList(currentPage, PageSize.pageComments);
            }

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
        public ActionResult Update(AddPostViewModel postModel, string tags)
        {
            Post post = GetPost(postModel.Id);

            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }

            if(!ModelState.IsValid)
            {
                Post emptyPost = new Post();
                return View("edit", emptyPost);
            }

            if (postModel.PostImageFile != null)
            {
                //upload a pic
                string fileName = Path.GetFileNameWithoutExtension(postModel.PostImageFile.FileName);
                string extension = Path.GetExtension(postModel.PostImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                postModel.PostImageUrl = "~/Images/Upload_Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Upload_Images"), fileName);
                postModel.PostImageFile.SaveAs(fileName);

                post.PostImageUrl = postModel.PostImageUrl;
                Session["PostImage"] = postModel.PostImageFile;
            }

            //edit
            post.Title = postModel.Title;
            post.EditTime = DateTime.Now;
            post.Body = postModel.Body;
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
            if (postModel.Id == 0)
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

        [HttpPost]
        public JsonResult PinComment(int id, int commentId)
        {
            var post = GetPost(id);
            post.PinCommentId = commentId;

            context.Entry(post).State = EntityState.Modified;
            bool result = context.SaveChanges() > 0;

            if (result)
                return Json(new { success = true });

            return Json(new { success = false , message = "OPPS! Something went wrong."});
        }

        [HttpPost]
        public ActionResult UnPinComment(int id)
        {
            var post = GetPost(id);
            post.PinCommentId = null;

            context.Entry(post).State = EntityState.Modified;
            bool result = context.SaveChanges() > 0;

             if (result)
                return Json(new { success = true });

             return Json(new { success = false, message = "OPPS! Something went wrong." });
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
            ViewBag.adminPic = AdminPicUrl();

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

        private string AdminPicUrl()
        {
            string adminUserName = (string) Session["AdminUserName"];

            if (!string.IsNullOrEmpty(adminUserName))
            {
                var admin = context.Administrators.Where(a => a.UserName.ToLower() == adminUserName.ToLower()).First();
                if (string.IsNullOrEmpty(admin.ProfilePic))
                    return "~/Images/default_user.jpg";

                return admin.ProfilePic;
            }
            else
                return context.Administrators.Where(a => a.UserName.ToLower() == DefaultAdmin.defaultAdminUserName).First().ProfilePic;
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

        //autocomplete search
        public JsonResult GetCompeletedSearch(string term)
        {
            var postComplete = context.Posts
                .Where(p => p.Title.StartsWith(term))
                .Select(p => p.Title)
                .ToList();

            return Json(postComplete, JsonRequestBehavior.AllowGet);
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
