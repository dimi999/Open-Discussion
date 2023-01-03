using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;

namespace OpenDiscussionv1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var roles = new List<IList<String>>();

            for (int i = 0; i < users.Count; i++)
            {
                roles.Add(await _userManager.GetRolesAsync(users[i]));
            }

            ViewBag.users = users;
            ViewBag.roles = roles;

            return View();
        }
    }
}
