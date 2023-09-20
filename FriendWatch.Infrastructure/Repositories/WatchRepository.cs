using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Infrastructure.Repositories
{
    public class WatchRepository : IWatchRepository
    {
        private readonly FriendWatchDbContext _context;

        public WatchRepository(FriendWatchDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Watch watch)
        {
            await _context.Watches.AddAsync(watch);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Watch>> GetByUserIdAsync(int currentUserId)
        {
            return await _context.Watches
                //.AsSplitQuery()
                .Include(x => x.Creator)
                .ThenInclude(y => y.Avatar)
                .Include(x => x.Comments)
                .ThenInclude(y => y.Commenter)
                .ThenInclude(z => z.Avatar)
                .Include(x => x.Circle)
                .ThenInclude(y => y.Members)
                .Where(x => x.Circle.Members.Any(y => y.Id == currentUserId))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
