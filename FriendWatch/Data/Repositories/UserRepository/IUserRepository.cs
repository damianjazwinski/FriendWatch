namespace FriendWatch.Data.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);
        Task<User> GetByUsername(string userName);
        Task<List<User>> GetAll();
        Task Create(User user);
        Task Update(User user);
        Task Delete(int id);
    }
}
