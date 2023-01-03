using OpenDiscussionv1.Models;

namespace OpenDiscussionv1.Infrastructure
{
    public interface IUnitOfWork
    {
        void UploadImage(IFormFile file, ApplicationUser user);
    }
}
