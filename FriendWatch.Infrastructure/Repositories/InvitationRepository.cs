using Microsoft.EntityFrameworkCore;
using FriendWatch.Application.Repositories;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly FriendWatchDbContext _context;

        public InvitationRepository(FriendWatchDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<Invitation?> GetByCircleIdAndReceiverIdAsync(int circleId, int receiverId)
        {
            return await _context.Invitations
                .SingleOrDefaultAsync(invitation => invitation.CircleId == circleId && invitation.ReceiverId == receiverId);
        }

        public async Task<Invitation?> GetByIdAsync(int id)
        {
            return await _context.Invitations
                .Include(x => x.Circle)
                .ThenInclude(y => y.Owner)
                .SingleOrDefaultAsync(invitation => invitation.Id == id);
        }

        public async Task<List<Invitation>> GetByReceiverIdAsync(int currentUserId)
        {
            return await _context.Invitations
                .Include(x => x.Circle)
                .ThenInclude(y => y.Owner)
                .Where(invitation => invitation.ReceiverId == currentUserId)
                .ToListAsync();
        }

        public async Task<List<Invitation>> GetBySenderIdAsync(int currentUserId)
        {
            return await _context.Invitations
                .Include(x => x.Circle)
                .ThenInclude(y => y.Owner)
                .Include(x => x.Receiver)
                .Where(invitation => invitation.Circle.OwnerId == currentUserId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Invitation invitation)
        {
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Invitation invitation)
        {
            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
