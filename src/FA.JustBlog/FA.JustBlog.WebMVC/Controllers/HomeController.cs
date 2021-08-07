using FA.JustBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FA.JustBlog.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostServices _postServices;

        public HomeController(IPostServices postServices)
        {
            _postServices = postServices;
        }
        public async Task<ActionResult> Index()
        {
            var posts = await _postServices.GetAllAsync();
            return View(posts);
        }

        public async Task<ActionResult> Detail(Guid id)
        {
            var post = await _postServices.GetByIdAsync(id);
            return View(post);
        }
        public  ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
           TempData["Infor"] = username + "-" + password;
            return RedirectToAction("Index");
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
    }
}