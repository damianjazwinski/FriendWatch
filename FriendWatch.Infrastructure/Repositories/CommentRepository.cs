using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Infrastructure.Persistence;

namespace FriendWatch.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly FriendWatchDbContext _context;

        public CommentRepository(FriendWatchDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Comment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
        }
    }
}
