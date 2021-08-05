using FA.JustBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            JustBlogDbContext db = new JustBlogDbContext();
            var a = db.Categories.ToList();
            a.ForEach(c => Console.WriteLine(c.Description));
            Console.ReadLine();
        }
    }
}
