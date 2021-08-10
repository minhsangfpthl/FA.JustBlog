using FA.JustBlog.Models.Common;
using FA.JustBlog.Services;
using FA.JustBlog.WebMVC2.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FA.JustBlog.WebMVC2.Areas.Admin.Controllers
{
    public class PostsManagementController : Controller
    {
        private readonly IPostServices _postServices;

        public PostsManagementController(IPostServices postServices)
        {
            _postServices = postServices;
        }

        // GET: Admin/PostsManagement
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageIndex = 1, int pageSize = 2)
        {
            ViewData["CurrentPageSize"] = pageSize;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSlugSortParm"] = sortOrder == "UrlSlug" ? "urlSlug_desc" : "UrlSlug";
            ViewData["TotalTagsSortParm"] = sortOrder == "TotalTags" ? "totalTags_desc" : "TotalTags";
            ViewData["InsertedAtSortParm"] = sortOrder == "InsertedAt" ? "insertedAt_desc" : "InsertedAt";
            ViewData["UpdatedAtSortParm"] = sortOrder == "UpdatedAt" ? "updatedAt_desc" : "UpdatedAt";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // x => x.Name.Contains(searchString)
            Expression<Func<Post, bool>> filter = null;

            if (!string.IsNullOrEmpty(searchString))
            {
                filter = c => c.Title.Contains(searchString);
            }

            // q => q.OrderByDescending(c => c.Name)
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null;

            switch (sortOrder)
            {
                case "name_desc":
                    orderBy = q => q.OrderByDescending(c => c.Title);
                    break;
                case "UrlSlug":
                    orderBy = q => q.OrderBy(c => c.UrlSlug);
                    break;
                case "urlSlug_desc":
                    orderBy = q => q.OrderByDescending(c => c.UrlSlug);
                    break;
                case "TotalTags":
                    orderBy = q => q.OrderBy(c => c.Tags.Count);
                    break;
                case "totalTags_desc":
                    orderBy = q => q.OrderByDescending(c => c.Tags.Count);
                    break;
                case "InsertedAt":
                    orderBy = q => q.OrderBy(c => c.InsertedAt);
                    break;
                case "insertedAt_desc":
                    orderBy = q => q.OrderByDescending(c => c.InsertedAt);
                    break;
                case "UpdatedAt":
                    orderBy = q => q.OrderBy(c => c.UpdatedAt);
                    break;
                case "updatedAt_desc":
                    orderBy = q => q.OrderByDescending(c => c.UpdatedAt);
                    break;
                default:
                    orderBy = q => q.OrderBy(c => c.Title);
                    break;
            }

            var posts = await _postServices.GetAsync(filter: filter, orderBy: orderBy, pageIndex: pageIndex ?? 1, pageSize: pageSize);

            return View(posts);
        }

        [HttpGet]
        // GET: Admin/PostsManagement/Create
        public ActionResult Create()
        {
            var postViewModel = new PostViewModels();
            return View(postViewModel);
        }

        // POST: Admin/PostsManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(PostViewModels postViewModel)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Title = postViewModel.Title,
                    ShortDescription = postViewModel.ShortDescription,
                    PostContent = postViewModel.PostContent,
                    UrlSlug = postViewModel.UrlSlug,
                    Published = postViewModel.Published,
                    PublishedDate = postViewModel.PublishedDate
                };
                var result = _postServices.Add(post);
                if (result > 0)
                {
                    TempData["Message"] = "Create post successful!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Create post failed! Please try again!";
                }
            }

            return View(postViewModel);
        }

        // GET: Admin/PostManagement/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = _postServices.GetById((Guid)id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var postViewModel = new PostViewModels()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                PostContent = post.PostContent,
                UrlSlug = post.UrlSlug,
                Published = post.Published,
                PublishedDate = post.PublishedDate
            };
            return View(postViewModel);
        }

        // POST: Admin/PostsManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(PostViewModels postViewModel)
        {
            if (ModelState.IsValid)
            {
                var post = await _postServices.GetByIdAsync(postViewModel.Id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                post.Title = post.Title;
                post.ShortDescription = post.ShortDescription;
                post.PostContent = post.PostContent;
                post.UrlSlug = post.UrlSlug;
                post.Published = post.Published;
                post.PublishedDate = post.PublishedDate;
                var result = _postServices.Update(post);
                if (result)
                {
                    TempData["Message"] = "Update successfully";
                }
                else
                {
                    TempData["Message"] = "Update failed";
                }
                return RedirectToAction("Index");
            }
            return View(postViewModel);
        }


        // POST: Admin/PostsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(Guid id)
        {
            Post post = _postServices.GetById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var result = _postServices.Delete(post.Id);
            if (result)
            {
                TempData["Message"] = "Delete Successful";
            }
            else
            {
                TempData["Message"] = "Delete Failed";
            }
            return RedirectToAction("Index");
        }
    }
}