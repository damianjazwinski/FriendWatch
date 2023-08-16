using Microsoft.EntityFrameworkCore;
using FriendWatch.Application.Repositories;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return;
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(x => x.Circles)
                .Include(x => x.OwnedCircles)
                .Include(x => x.ReceivedInvitations)
                .SingleOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            return user;
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
