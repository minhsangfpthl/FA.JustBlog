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
    [Authorize]
    public class PostsManagementController : Controller
    {
        private readonly IPostServices _postServices;
        private readonly ICategoryServices _categoryServices;
        private readonly ITagServices _tagServices;

        public PostsManagementController(IPostServices postServices, ICategoryServices categoryServices,
                                            ITagServices tagServices)
        {
            _postServices = postServices;
            _categoryServices = categoryServices;
            _tagServices = tagServices;
        }

        // GET: Admin/PostsManagement
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageIndex = 1, int pageSize = 2)
        {
            ViewData["CurrentPageSize"] = pageSize;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
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
                case "title_desc":
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
            ViewBag.Categories = new SelectList(_categoryServices.GetAll(), "Id", "Name");
            var postViewModel = new PostViewModels();
            postViewModel.Tags = _tagServices.GetAll().Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
            return View(postViewModel);
        }

        // POST: Admin/PostsManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(PostViewModels postViewModel)
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
                    PublishedDate = postViewModel.PublishedDate,
                    CategoryId = postViewModel.CategoryId,
                    Tags = await GetSelectedTagFromIds(postViewModel.SelectedTagIds)
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

            ViewBag.Categories = new SelectList(await _categoryServices.GetAllAsync(), "Id", "Name", postViewModel.CategoryId);
            postViewModel.Tags = _tagServices.GetAll().Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
            return View(postViewModel);
        }

        private async Task<ICollection<Tag>> GetSelectedTagFromIds(IEnumerable<Guid> selectedTagIds)
        {
            var tags = new List<Tag>();

            if (selectedTagIds == null)
            {
                return tags;
            }
            var tagEntities = await _tagServices.GetAllAsync();

            foreach (var item in tagEntities)
            {
                if (selectedTagIds.Any(x => x == item.Id))
                {
                    tags.Add(item);
                }
            }
            return tags;
        }

        // GET: Admin/PostManagement/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
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
                PublishedDate = post.PublishedDate,
                CategoryId = post.CategoryId,
                SelectedTagIds = post.Tags.Select(x => x.Id)
            };
            ViewBag.Categories = new SelectList(_categoryServices.GetAll(), "Id", "Name");
            postViewModel.Tags = _tagServices.GetAll().Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
            ViewBag.TagList = _tagServices.GetAll();
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
                post.Published = postViewModel.Published;
                post.CategoryId = postViewModel.CategoryId;
                await UpdateSelectedTagFromIds(postViewModel.SelectedTagIds, post);

                var result = await _postServices.UpdateAsync(post);
                if (result)
                {
                    TempData["Message"] = "Update successful!";
                }
                else
                {
                    TempData["Message"] = "Update failed!";

                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(await _categoryServices.GetAllAsync(), "Id", "Name", postViewModel.CategoryId);
            postViewModel.Tags = _tagServices.GetAll().Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
            ViewBag.TagList = _tagServices.GetAll();
            return View(postViewModel);
        }

        private async Task UpdateSelectedTagFromIds(IEnumerable<Guid> selectedTagIds, Post post)
        {
            var tags = post.Tags;
            foreach (var item in tags.ToList())
            {
                post.Tags.Remove(item);
            }
            post.Tags = await GetSelectedTagFromIds(selectedTagIds);
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