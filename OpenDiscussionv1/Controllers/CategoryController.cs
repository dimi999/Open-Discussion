using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;

namespace OpenDiscussionv1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoryController(ApplicationDbContext db)
        {
            this.db = db;
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
