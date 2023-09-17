using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IWatchService
    {
        Task<ServiceResponse<WatchDto>> CreateWatchAsync(WatchDto watchDto);
    }
}
