using FA.JustBlog.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FA.JustBlog.WebMVC2.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IPostServices _postServices;
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(IPostServices postServices, ICategoryServices categoryServices)
        {
            _postServices = postServices;
            _categoryServices = categoryServices;
        }

        public async Task<ActionResult> Details(Guid id)
        {

            var category = await _categoryServices.GetByIdAsync(id);

            var posts = await _postServices.GetPostsByCategoryAsync(id);
            ViewBag.CategoryName = category.Name;
            return View(posts);
        }
    }
}