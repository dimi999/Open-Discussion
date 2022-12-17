using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System;

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
            return View();
        }

        public IActionResult Show()
        {
            List<Category> categ = db.Categories.ToList();
            ViewBag.Categories = categ;
            
            return View();
        }

        public IActionResult Insert(Category categ)
        {
            try
            {
                db.Categories.Add(categ);
                db.SaveChanges();
                return RedirectToAction("Show");
            } catch
            {
                return RedirectToAction("Invalid");
            }
        }

        public IActionResult Delete(string id)
        {
            int id_bun = Int32.Parse(id);
            try
            {
                db.Categories.Remove(db.Categories.Find(id_bun));
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            catch
            {
                return RedirectToAction("Invalid");
            }
        }

    }
}
