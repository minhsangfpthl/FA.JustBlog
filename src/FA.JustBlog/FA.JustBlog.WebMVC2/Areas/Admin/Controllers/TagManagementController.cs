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
    public class TagManagementController : Controller
    {
        private readonly ITagServices _tagServices;

        public TagManagementController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }
        // GET: Admin/TagManagement
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageIndex = 1, int pageSize = 2)
        {
            ViewData["CurrentPageSize"] = pageSize;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSlugSortParm"] = sortOrder == "UrlSlug" ? "urlSlug_desc" : "UrlSlug";
            ViewData["TotalPostsSortParm"] = sortOrder == "TotalPosts" ? "totalPosts_desc" : "TotalPosts";
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
            Expression<Func<Tag, bool>> filter = null;

            if (!string.IsNullOrEmpty(searchString))
            {
                filter = c => c.Name.Contains(searchString);
            }

            // q => q.OrderByDescending(c => c.Name)
            Func<IQueryable<Tag>, IOrderedQueryable<Tag>> orderBy = null;

            switch (sortOrder)
            {
                case "name_desc":
                    orderBy = q => q.OrderByDescending(c => c.Name);
                    break;
                case "UrlSlug":
                    orderBy = q => q.OrderBy(c => c.UrlSlug);
                    break;
                case "urlSlug_desc":
                    orderBy = q => q.OrderByDescending(c => c.UrlSlug);
                    break;
                case "TotalPosts":
                    orderBy = q => q.OrderBy(c => c.Posts.Count);
                    break;
                case "totalPosts_desc":
                    orderBy = q => q.OrderByDescending(c => c.Posts.Count);
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
                    orderBy = q => q.OrderBy(c => c.Name);
                    break;
            }

            var tags = await _tagServices.GetAsync(filter: filter, orderBy: orderBy, pageIndex: pageIndex ?? 1, pageSize: pageSize);

            return View(tags);
        }

        [HttpGet]
        // GET: Admin/TagManagement/Create
        public ActionResult Create()
        {
            var tagViewModel = new TagViewModel();
            return View(tagViewModel);
        }

        // POST: Admin/TagManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TagViewModel tagViewModel)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = tagViewModel.Name,
                    UrlSlug = tagViewModel.UrlSlug,
                    Description = tagViewModel.Description,
                    Count = tagViewModel.Count
                };
                var result = _tagServices.Add(tag);
                if (result > 0)
                {
                    TempData["Message"] = "Create tag successful!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Create tag failed! Please try again!";
                }
            }

            return View(tagViewModel);
        }


        // GET: Admin/TagManagement/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tag = _tagServices.GetById((Guid)id);
            if (tag == null)
            {
                return HttpNotFound();
            }

            var tagViewModel = new TagViewModel()
            {
                Id = tag.Id,
                Name = tag.Name,
                UrlSlug = tag.UrlSlug,
                Description = tag.Description,
                Count = tag.Count
            };
            return View(tagViewModel);
        }

        // POST: Admin/TagManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(TagViewModel tagViewModel)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagServices.GetByIdAsync(tagViewModel.Id);
                if (tag == null)
                {
                    return HttpNotFound();
                }
                tag.Name = tagViewModel.Name;
                tag.UrlSlug = tagViewModel.UrlSlug;
                tag.Description = tagViewModel.Description;
                tag.Count = tagViewModel.Count;

                var result = _tagServices.Update(tag);
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
            return View(tagViewModel);
        }

        // POST: Admin/TagManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(Guid id)
        {
            Tag tag = _tagServices.GetById(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            var result = _tagServices.Delete(tag.Id);
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