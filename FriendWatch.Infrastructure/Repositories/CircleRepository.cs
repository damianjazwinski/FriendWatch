using Microsoft.EntityFrameworkCore;
using FriendWatch.Application.Repositories;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Repositories
{
    public class CircleRepository : ICircleRepository
    {
        private readonly DataContext _context;
        public CircleRepository(DataContext context) 
        {
            _context = context;
        }
        public async Task CreateAsync(Circle circle)
        {
            await _context.Circles.AddAsync(circle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Circle circle)
        {
            _context.Circles.Remove(circle);
            await _context.SaveChangesAsync();
        }

        public async Task<Circle> GetByIdAsync(int id)
        {
            return await _context.Circles.Include(x => x.Owner).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Circle> GetByNameAsync(string name)
        {
            return await _context.Circles.Include(x => x.Owner).SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Circle> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Circles.Include(x => x.Owner).SingleOrDefaultAsync(x => x.Owner.Id == ownerId);
        }

        public Task UpdateAsync(Circle circle)
        {
            throw new NotImplementedException();
        }
    }
}
