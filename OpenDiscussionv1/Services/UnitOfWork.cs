using OpenDiscussionv1.Infrastructure;
using OpenDiscussionv1.Models;
using System.Drawing;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace OpenDiscussionv1.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private IHostingEnvironment _env;

        public UnitOfWork(IHostingEnvironment env)
        {
            _env = env;
        }

        public async void UploadImage(IFormFile file, ApplicationUser user)
        {
            if (file != null)
            {
                var storagePath = Path.Combine(_env.WebRootPath, "images", user.Id + ".jpeg");
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
