using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Services
{
    public class WatchService : IWatchService
    {
        private readonly IWatchRepository _watchRepository;

        public WatchService(IWatchRepository watchRepository)
        {
            _watchRepository = watchRepository;
        }

        public async Task<ServiceResponse<WatchDto>> CreateWatchAsync(WatchDto watchDto)
        {
            var watch = new Watch
            {
                CreatorId = watchDto.CreatorId,
                CircleId = watchDto.CircleId,
                Message = watchDto.Message,
                ExternalLink = watchDto.ExternalLink,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _watchRepository.CreateAsync(watch);

            return new ServiceResponse<WatchDto>(true);
        }
    }
}
