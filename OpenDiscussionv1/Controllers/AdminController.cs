using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Data;
using OpenDiscussionv1.Models;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;

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
            var roles = new List<IList<string>>();

            for (int i = 0; i < users.Count; i++)
            {
                roles.Add(await _userManager.GetRolesAsync(users[i]));
            }

            ViewBag.users = users;
            ViewBag.roles = roles;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles;

            string current_role = (await _userManager.GetRolesAsync(user))[0];

            ViewBag.user = user;
            ViewBag.roles = roles;
            ViewBag.current_role = current_role;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(IFormCollection ifc)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(ifc["user_id"][0]);
                string current_role = (await _userManager.GetRolesAsync(user))[0];

                IdentityRole role = await _roleManager.FindByIdAsync(ifc["role_id"][0]);

                await _userManager.RemoveFromRoleAsync(user, current_role);
                await _userManager.AddToRoleAsync(user, role.Name);

                await _userManager.UpdateSecurityStampAsync(user);

                TempData["message"] = "Rolul a fost editat cu succes";   
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Eroare la editarea rolului";
                return RedirectToAction("Index");
            }
        }
    }
}
