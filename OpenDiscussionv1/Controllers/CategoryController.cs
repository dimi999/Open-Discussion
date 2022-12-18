using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;

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
                        select category;
            ViewBag.Categories = categ;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult New(Category categ)
        {
            try
            {
                db.Categories.Add(categ);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
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
                Category category = db.Categories.Find(id);
                category.CategoryName = requestCategory.CategoryName;
                db.SaveChanges();

                TempData["message"] = "Categoria a fost editata!";
                return RedirectToAction("Index");
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
