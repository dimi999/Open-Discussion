using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;

namespace OpenDiscussionv1.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db;

        public SearchController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            string search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();

                var discussions = db.Discussions.Where(
                        disc => disc.Title.Contains(search) || disc.Content.Contains(search)
                    ).Select(disc => disc).Include("User").Include("Category");

                var replies = db.Replies.Where(
                        reply => reply.Content.Contains(search)
                    ).Select(reply => reply).Include("User").Include("Discussion");

                ViewBag.Search = search;
                ViewBag.Discussions = discussions;
                ViewBag.Discussions_Count = discussions.Count();
                ViewBag.Replies = replies;
                ViewBag.Replies_Count = replies.Count();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Category");
            }
        }
    }
}
