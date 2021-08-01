using FA.JustBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FA.JustBlog.WebMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostServices _postServices;
        public PostController(IPostServices postServices)
        {
            _postServices = postServices;
        }
        // GET: Posts
        public async Task<ActionResult> Index()
        {
            //Expression<Func<PostsController,bool>> filter
            var posts = await _postServices.GetAsync();
            return View(posts);
        }
    }
}