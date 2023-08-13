namespace FriendWatch.Data.Repositories.CircleRepository
{
    public interface ICircleRepository
    {
        Task<Circle> GetByIdAsync(int id);
        Task<Circle> GetByNameAsync(string name);
        Task<Circle> GetByOwnerIdAsync(int ownerId);
        Task CreateAsync(Circle circle);
        Task UpdateAsync(Circle circle);
        Task DeleteAsync(Circle circle);
    }
}
