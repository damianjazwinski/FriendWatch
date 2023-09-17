using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Infrastructure.Persistence;

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
    }
}
