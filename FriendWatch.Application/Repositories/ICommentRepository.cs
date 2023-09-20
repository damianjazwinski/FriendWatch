using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
    }
}
