using FA.JustBlog.Models.Common;
using FA.JustBlog.Services;
using FA.JustBlog.WebMVC2.Areas.Admin.ViewModels;
using FA.JustBlog.WebMVC2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FA.JustBlog.WebMVC2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostServices _postServices;
        private readonly ICategoryServices _categoyServices;

        public HomeController(IPostServices postServices, ICategoryServices categoyServices)
        {
            _postServices = postServices;
            _categoyServices = categoyServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string searchString, string currentFilter, int? pageIndex = 1, int? pageSize = 3)
        {
            ViewData["CurrentPageSize"] = pageSize;

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

            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = x => x.OrderByDescending(p => p.PublishedDate);

            var posts = await _postServices.GetAsync(filter: filter, orderBy: orderBy, pageIndex: pageIndex ?? 1, pageSize: pageSize ?? 3);

            return View(posts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CategoryMenu()
        {
            var categoryViewModels = _categoyServices.GetAll().Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();


            return PartialView("_CategoryMenu", categoryViewModels);
        }

        public ActionResult Menu()
        {
            var categories = _categoyServices.GetAll();
            var popularCategories = categories.OrderByDescending(x => x.Posts.Count).Take(3);
            var rightCategories = categories.OrderByDescending(x => x.Posts.Count).Skip(3);
            var categoryMenuViewModel = new CategoryMenuViewModel()
            {
                PopularCategory = popularCategories,
                rightCategories = rightCategories
            };
            return PartialView("_CategoryMenu", categoryMenuViewModel);
        }
    }
}