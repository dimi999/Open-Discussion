using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;
using System.Diagnostics;

namespace OpenDiscussionv1.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DiscussionController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult View(int id)
        {
            var discussion = db.Discussions
                            .Include(discussion => discussion.User)
                            .Include(discussion => discussion.Category)
                            .Include(discussion => discussion.Replies)
                            .ThenInclude(reply => reply.User)
                            .FirstOrDefault(d => d.DiscussionId == id);
            ViewBag.Discussion = discussion;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public IActionResult _View(Discussion discussion)
        {
            ViewBag.Discussion = discussion;

            var categ = from category in db.Categories
                        select category;
            ViewBag.Categories = categ;

            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpGet]
        public IActionResult New()
        {
            var categ = from category in db.Categories
                        select category;
            ViewBag.Categories = categ;

            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult New(Discussion discussion)
        {
            discussion.CreatedAt = DateTime.Now;
            discussion.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                try
                {
                    db.Discussions.Add(discussion);
                    db.SaveChanges();
                    TempData["message"] = "Discutia a fost creata!";
                    return RedirectToAction("View", "Category", new { id = discussion.CategoryId });
                }
                catch 
                {
                    TempData["message"] = "Eroare la adaugarea discutiei!";
                    return RedirectToAction("View", "Category", new {id = discussion.CategoryId});
                }
            }
            else
            {
                return _View(discussion);
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Discussion discussion = db.Discussions.Find(id);
            if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                try
                {
                    ViewBag.Discussion = discussion;

                    var categ = from category in db.Categories
                                select category;
                    ViewBag.Categories = categ;

                    return View();
                }
                catch
                {
                    TempData["message"] = "Discutia nu a fost gasita!";
                    return RedirectToAction("View", "Category", new { id = discussion.CategoryId });
                }
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", new { id = discussion.DiscussionId });
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Discussion requestDiscussion)
        {
            Discussion discussion = db.Discussions.Find(id);
            if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        discussion.Title = requestDiscussion.Title;
                        discussion.Content = requestDiscussion.Content;
                        discussion.CategoryId = requestDiscussion.CategoryId;
                        db.SaveChanges();

                        TempData["message"] = "Discutia a fost editata!";
                        return RedirectToAction("View", new { id = discussion.DiscussionId });
                    }
                    else
                    {
                        TempData["message"] = "Intrari invalide!";
                        return RedirectToAction("View", new { id = discussion.DiscussionId });
                    }
                }
                catch
                {
                    TempData["message"] = "Eroare la editarea discutiei!";
                    return RedirectToAction("View", new { id = discussion.DiscussionId });
                }
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", new { id = discussion.DiscussionId });
            } 
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Discussion discussion = db.Discussions.Find(id);

            if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                try
                {
                    db.Discussions.Remove(discussion);
                    db.SaveChanges();
                    TempData["message"] = "Discutia a fost stearsa!";
                    return RedirectToAction("View", "Category", new { id = discussion.CategoryId });
                }
                catch
                {
                    TempData["message"] = "Eroare la stergerea discutiei!";
                    return RedirectToAction("View", "Category", new { id = discussion.CategoryId });
                }
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                return RedirectToAction("View", "Category", new { id = discussion.CategoryId });
            }
        }
    }
}
