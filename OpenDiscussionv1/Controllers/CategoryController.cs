using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;
using System.Diagnostics;

namespace OpenDiscussionv1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CategoryController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var categ = from category in db.Categories
                        select new
                        {
                            Category = category,
                            No_Discussions = (
                                from discussion in db.Discussions
                                where discussion.CategoryId == category.CategoryId
                                select discussion
                            ).Count()
                        };

            ViewBag.Categories = categ;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public IActionResult View(int id)
        {
            Category category = db.Categories.Find(id);

            var No_Discussions = (
                        from discussion in db.Discussions
                        where discussion.CategoryId == category.CategoryId
                        select discussion
                    ).Count();

            int _perPage = 3;
            var currentCriteria = 0;
            if (HttpContext.Request.Query.ContainsKey("criteria"))
                currentCriteria = Convert.ToInt32(HttpContext.Request.Query["criteria"]);
            var discussions = db.Discussions
                    .Where(d => d.CategoryId == category.CategoryId)
                    .Include("User");
            if (currentCriteria == 2)
            {
                discussions = db.Discussions
                    .Where(d => d.CategoryId == category.CategoryId)
                    .Include("User").OrderByDescending(x => x.Replies.Count());
            }
            else
            {
                discussions = db.Discussions
                    .Where(d => d.CategoryId == category.CategoryId)
                    .Include("User").OrderByDescending(x => x.CreatedAt);
            }

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            int totalItems = discussions.Count();
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }
            var paginatedDiscussions = discussions.Skip(offset).Take(_perPage);
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
            ViewBag.Discussions = paginatedDiscussions;

            ViewBag.Criteria = currentCriteria;
            ViewBag.Category = category;
            ViewBag.No_Discussions = No_Discussions;

            return View();
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult New(Category categ)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    db.Categories.Add(categ);
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Intrari invalide!";
                    return RedirectToAction("Index");
                }
            } catch
            {
                TempData["message"] = "Eroare la adaugarea categoriei!";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                ViewBag.Category = category;
                return View();
            } catch
            {
                TempData["message"] = "Categoria nu a fost gasita!";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);
                    category.CategoryName = requestCategory.CategoryName;
                    db.SaveChanges();

                    TempData["message"] = "Categoria a fost editata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Intrari invalide!";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["message"] = "Eroare la editarea categoriei!";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost stearsa!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Eroare la stergerea categoriei!";
                return RedirectToAction("Index");
            }
        }

    }
}
