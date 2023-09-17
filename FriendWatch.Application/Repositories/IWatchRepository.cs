using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IWatchRepository
    {
        Task CreateAsync(Watch watch);
    }
}
