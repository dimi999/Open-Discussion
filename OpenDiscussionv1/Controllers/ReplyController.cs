using Microsoft.AspNetCore.Mvc;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Diagnostics;

namespace OpenDiscussionv1.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReplyController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult New(Reply reply)
        {
            reply.CreatedAt = DateTime.Now;

            try
            {
                db.Replies.Add(reply);
                db.SaveChanges();

                TempData["message"] = "Raspunsul a fost adaugat!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
            catch
            {
                TempData["message"] = "Eroare la adaugarea raspunsului!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Reply reply = db.Replies.Find(id);
                ViewBag.Reply = reply;

                return View();
            }
            catch
            {
                TempData["message"] = "Raspunsul nu a fost gasita!";
                return RedirectToAction("Index", "Discussion");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, Reply requestReply)
        {
            Reply reply = db.Replies.Find(id);

            try
            {    
                reply.Content = requestReply.Content;
                db.SaveChanges();

                TempData["message"] = "Raspunsul a fost editat!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
            catch
            {
                TempData["message"] = "Eroare la editarea raspunsului!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Reply reply = db.Replies.Find(id);
            int discussionId = reply.DiscussionId;

            try
            {    
                db.Replies.Remove(reply);
                db.SaveChanges();

                TempData["message"] = "Raspunsul a fost sters!";
                return RedirectToAction("View", "Discussion", new { id = discussionId });
            }
            catch
            {
                TempData["message"] = "Eroare la stergerea raspunsului!";
                return RedirectToAction("View", "Discussion", new { id = discussionId });
            }
        }
    }
}
