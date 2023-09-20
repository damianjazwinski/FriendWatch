using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Infrastructure.Repositories
{
    public class CircleRepository : ICircleRepository
    {
        private readonly FriendWatchDbContext _context;
        public CircleRepository(FriendWatchDbContext context)
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

        public async Task<Circle?> GetByIdAsync(int id)
        {
            return await _context.Circles.Include(x => x.ImageFile).Include(x => x.Owner).Include(x => x.Members).ThenInclude(y => y.Avatar).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Circle?> GetByOwnerIdAndNameAsync(int ownerId, string name)
        {
            return await _context.Circles.Include(x => x.ImageFile).Include(x => x.Owner).SingleOrDefaultAsync(x => x.OwnerId == ownerId && x.Name == name);
        }

        public async Task<List<Circle>> GetByNameAsync(string name)
        {
            return await _context.Circles.Include(x => x.ImageFile).Include(x => x.Owner).Where(x => x.Name == name).ToListAsync();
        }

        public async Task<List<Circle>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Circles.Include(x => x.ImageFile).Include(x => x.Owner).Where(x => x.OwnerId == ownerId).ToListAsync();
        }

        public async Task UpdateAsync(Circle circle)
        {
            _context.Circles.Update(circle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Circle>> GetJoinedAsync(int currentUserId)
        {
            return await _context.Circles.Include(x => x.Owner).Include(y => y.Members).Where(circle => circle.Members.Any(member => member.Id == currentUserId)).ToListAsync();
        }
    }
}
