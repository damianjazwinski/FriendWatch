using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
    }
}
