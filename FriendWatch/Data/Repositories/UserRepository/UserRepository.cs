using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task Create(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<User> GetByUsername(string userName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userName);
            return user;
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
