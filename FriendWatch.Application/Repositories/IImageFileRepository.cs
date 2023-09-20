using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IImageFileRepository
    {
        Task AddAsync(ImageFile imageFile);
        Task<ImageFile?> GetImageFileAsync(int id);
        Task<ImageFile?> GetImageFileAsync(string fileName);
    }
}
