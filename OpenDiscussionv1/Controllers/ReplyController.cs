using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;
using System.Diagnostics;

namespace OpenDiscussionv1.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReplyController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult New(Reply reply)
        {
            reply.CreatedAt = DateTime.Now;
            reply.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
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
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }  
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Reply reply = db.Replies.Find(id);
            if (reply.UserId == _userManager.GetUserId(User) ||User.IsInRole("Admin"))
            {
                try
                {
                    ViewBag.Reply = reply;

                    return View();
                }
                catch
                {
                    TempData["message"] = "Raspunsul nu a fost gasita!";
                    return RedirectToAction("Index", "Discussion");
                }
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Reply requestReply)
        {
            Reply reply = db.Replies.Find(id);
            if (reply.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
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
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Reply reply = db.Replies.Find(id);
            int? discussionId = reply.DiscussionId;

            if (reply.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
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
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", "Discussion", new { id = reply.DiscussionId });
            }  
        }
    }
}
