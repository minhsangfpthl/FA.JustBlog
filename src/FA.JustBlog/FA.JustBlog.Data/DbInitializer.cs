using FA.JustBlog.Models.Common;
using FA.JustBlog.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FA.JustBlog.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<JustBlogDbContext>
    {
        protected override void Seed(JustBlogDbContext context)
        {
            InitializeIdentity(context);
            var categories = new Category[]
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Travel",
                    UrlSlug =   "travel-blog",
                    Description ="TravelBlog was developed to help people share their real life travel experiences with friends," +
                    " family and other travelers. Although there have been many additions to the services TravelBlog.org offers," +
                    " the website retains this principle and remains both free to use and independent.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Recipe",
                    UrlSlug =   "recipe-blog",
                    Description ="Each recipe is attached with a photo of the final product, along with several photos that portray each major step " +
                    "in the recipe. The website's posts are organized in categories, such as one that only includes recipes involving beef, appetizers," +
                    " or low-budget meals.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Tips",
                    UrlSlug =   "tips",
                    Description ="Tips Blog",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Life Style",
                    UrlSlug =   "life-style",
                    Description ="Life Style Blog",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Food",
                    UrlSlug =   "food-blog",
                    Description ="Food blogging represents a complex interweaving of “foodie” or gourmet interest in cooking with those of " +
                    "blog writing and photography. The majority of blogs use pictures taken by the author himself/herself and some of them focus" +
                    " specifically on food photography.",
                    IsDeleted = false
                }
            };

            var tag1 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "travel",
                UrlSlug = "travel",
                Description = "Travel Tag",
                IsDeleted = false
            };

            var tag2 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "food",
                UrlSlug = "food",
                Description = "food Tag",
                IsDeleted = false
            };

            var tag3 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "recipe",
                UrlSlug = "recipe",
                Description = "recipe Tag",
                IsDeleted = false
            };

            var tag4 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "tips",
                UrlSlug = "tips",
                Description = "tips Tag",
                IsDeleted = false
            };

            var tag5 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "study",
                UrlSlug = "study",
                Description = "study Tag",
                IsDeleted = false
            };

            var tag6 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "life style",
                UrlSlug = "life-style",
                Description = "life style Tag",
                IsDeleted = false
            };

            var tag7 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "setup",
                UrlSlug = "setup",
                Description = "setup Tag",
                IsDeleted = false
            };

            var posts = new List<Post>
            {
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "What’s it Like to Live in Germany – the Good, Bad and the FUN",
                    UrlSlug = "what-its-like-to-live-in-germany",
                    ShortDescription = "It has now been close to five years that I have been living in Germany",
                    ImageUrl = "blog-1.jpg",
                    PostContent = "Every country has rules for everything, but not everyone follows them. In many cases, the people aren’t even aware" +
                    " of the rules in the first place. Things run very differently in Germany. In fact, things run exactly the way they should.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[0].Name),
                    Tags = new List<Tag>{tag1, tag2,tag3}
                },
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Bosnia Road Trip: Itinerary for Bosnia-Herzegovina [10 Days] in the Balkans",
                    UrlSlug = "bosnia-road-trip-itinerary",
                    ShortDescription = "How awesome is Bosnia-Herzegovina – there are epic waterfalls, stunning clear rivers and charming little towns.",
                    ImageUrl = "blog-2.jpg",
                    PostContent = "When considering a country in eastern Europe for a road trip, Bosnia & Herzegovina should be one of the first " +
                    "countries on your mind. This idyllic country often gets overshadowed by its neighbour, Croatia. Don’t get me wrong, Croatia is " +
                    "a magnificent country in itself but Bosnia & Herzegovina is just something else.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[3].Name),
                    Tags = new List<Tag>{tag1, tag4,tag3}
                },
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Northern Italy by Train Itinerary: Where to Go + How to do it + Info",
                    UrlSlug = "northern-italy-by-train",
                    ShortDescription = "Italy has always been the centre of attention because of its beauty and culture. Year after year, it ranks" +
                    " in the top 10 as one of the most visited countries in the world",
                    ImageUrl = "blog-3.jpg",
                    PostContent = "Italy’s amazingness isn’t just due to its rich culture or fascinating history, or even the scenic beauty. " +
                    "It is more than that. It is about experiencing the mediterranean climate, the joy of sitting on a chair along the street in one " +
                    "of the cafes while sipping espresso, tasting the simplicity of food that’s cooked with just 3-5 ingredients, seeing the locals" +
                    " communicate with energetic gestures and listening to the musical sound of Italian chatter.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[1].Name),
                    Tags = new List<Tag>{tag5, tag2,tag3}
                },
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Turkey Travel Tips (from a local): 15 Things to Know Before Visiting",
                    UrlSlug = "turkey-travel-tips",
                    ShortDescription = "Turkey travel tips has been written our Europe content specialist – Alara Benlier, who is originally from" +
                    " Turkey. This post has been further expanded by the editor.",
                    ImageUrl = "blog-4.jpg",
                    PostContent = "Turkey is historical, vibrant, and is insanely beautiful. This country that’s twice the size of California " +
                    "offers an exhaustive selection of places to visit and travel experiences to its visitors.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[2].Name),
                    Tags = new List<Tag>{tag1, tag5,tag3}
                },
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "The Spectacular Lake Bohinj in Triglav National Park, Slovenia [Bohinjsko Jezero]",
                    UrlSlug = "lake-bohinj-slovenia",
                    ShortDescription = "Imagine a lake with clear blue-green water that’s surrounded by mountains. To make things even better, " +
                    "there are multiple beaches where one can chill and enjoy the magic of Mother Nature.",
                    ImageUrl = "blog-5.jpg",
                    PostContent = "We ended up visiting and camping next to Lake Bohinj by just chance. Honestly, we did not even know about" +
                    " this awesome lake but we were looking for a place to camp in Slovenia. At that time, we were on our camper van" +
                    " and driving to Croatia. We saw a big lake marked as “Bohinjsko jezero” on Google Maps, saw the pictures " +
                    "and immediately decided to check it out.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[1].Name),
                    Tags = new List<Tag>{tag2,tag3}
                },
                new Post
                {
                    Id = Guid.NewGuid(),
                    Title = "The Ultimate Hamburg Nightlife Guide: Top Clubs + Survival tips",
                    UrlSlug = "hamburg-nightlife",
                    ShortDescription = "Hamburg Nightlife Guide has been written our Germany and Netherlands content specialist – Alara Benlier." +
                    " This post has been further expanded by the editor. ",
                    ImageUrl = "blog-6.jpg",
                    PostContent = "Young, hip and fun – those are just some words that can describe the “Gateway to the World”, aka Hamburg." +
                    " Located along the River Elbe, this radiant city has the second busiest harbor in Europe.",
                    PublishedDate = DateTime.Now,
                    IsDeleted = false,
                    Published = true,
                    Category = categories.Single(x => x.Name == categories[0].Name),
                    Tags = new List<Tag>{tag6,tag3}
                }
            };
            context.Categories.AddRange(categories);
            context.Posts.AddRange(posts);
            context.SaveChanges();
        }

        public static void InitializeIdentity(JustBlogDbContext db)
        {
            var userManager = new UserManager<User>(new UserStore<User>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new User { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}