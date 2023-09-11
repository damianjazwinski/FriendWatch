using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface ICircleRepository
    {
        Task<Circle?> GetByIdAsync(int id);
        Task<Circle?> GetByOwnerIdAndNameAsync(int ownerId, string name);
        Task<List<Circle>> GetByNameAsync(string name);
        Task<List<Circle>> GetByOwnerIdAsync(int ownerId);
        Task CreateAsync(Circle circle);
        Task UpdateAsync(Circle circle);
        Task DeleteAsync(Circle circle);
        Task<List<Circle>> GetJoinedAsync(int currentUserId);
    }
}
