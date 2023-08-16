using Microsoft.EntityFrameworkCore;
using FriendWatch.Application.Repositories;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly DataContext _context;

        public InvitationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
