using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Diagnostics;

namespace OpenDiscussionv1.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly ApplicationDbContext db;

        public DiscussionController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var discussions = db.Discussions.Include("Category");
            ViewBag.Discussions = discussions;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public IActionResult View(int id)
        {
            var discussion = db.Discussions
                            .Include("Category")
                            .Include("Replies")
                            .FirstOrDefault(d => d.DiscussionId == id);
            ViewBag.Discussion = discussion;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            var categ = from category in db.Categories
                        select category;
            ViewBag.Categories = categ;

            return View();
        }

        [HttpPost]
        public IActionResult New(Discussion discussion)
        {
            discussion.CreatedAt = DateTime.Now;

            try
            {
                db.Discussions.Add(discussion);
                db.SaveChanges();
                TempData["message"] = "Discutia a fost creata!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Eroare la adaugarea discutiei!";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Discussion discussion = db.Discussions.Find(id);
                ViewBag.Discussion = discussion;

                var categ = from category in db.Categories
                            select category;
                ViewBag.Categories = categ;

                return View();
            }
            catch
            {
                TempData["message"] = "Discutia nu a fost gasita!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, Discussion requestDiscussion)
        {
            Discussion discussion = db.Discussions.Find(id);

            try
            {    
                discussion.Title = requestDiscussion.Title;
                discussion.Content = requestDiscussion.Content;
                discussion.CategoryId = requestDiscussion.CategoryId;
                db.SaveChanges();

                TempData["message"] = "Discutia a fost editata!";
                return RedirectToAction("View", new { id = discussion.DiscussionId });
            }
            catch
            {
                TempData["message"] = "Eroare la editarea discutiei!";
                return RedirectToAction("View", new { id = discussion.DiscussionId });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                Discussion discussion = db.Discussions.Find(id);
                db.Discussions.Remove(discussion);
                db.SaveChanges();
                TempData["message"] = "Discutia a fost stearsa!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Eroare la stergerea discutiei!";
                return RedirectToAction("Index");
            }
        }
    }
}
