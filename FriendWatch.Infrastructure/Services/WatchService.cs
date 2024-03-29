﻿using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Infrastructure.Services
{
    public class WatchService : IWatchService
    {
        private readonly IWatchRepository _watchRepository;
        private readonly ICommentRepository _commentRepository;

        public WatchService(IWatchRepository watchRepository, ICommentRepository commentRepository)
        {
            _watchRepository = watchRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ServiceResponse<WatchDto>> AddCommentToWatch(CommentDto commentDto)
        {
            var comment = new Comment
            {
                CommenterId = commentDto.CommenterId,
                WatchId = commentDto.WatchId,
                Content = commentDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _commentRepository.CreateAsync(comment);

            return new ServiceResponse<WatchDto>(true);
        }

        public async Task<ServiceResponse<WatchDto>> CreateWatchAsync(WatchDto watchDto)
        {
            var watch = new Watch
            {
                CreatorId = watchDto.CreatorId,
                CircleId = watchDto.CircleId,
                Message = watchDto.Message,
                ExternalLink = watchDto.ExternalLink,
                ExpirationDate = watchDto.ExpirationDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _watchRepository.CreateAsync(watch);

            return new ServiceResponse<WatchDto>(true);
        }

        public async Task<ServiceResponse<List<WatchDto>>> GetAllWatchesForUserAsync(int currentUserId)
        {
            var watches = await _watchRepository.GetByUserIdAsync(currentUserId);

            if (watches == null)
                return new ServiceResponse<List<WatchDto>>(false, null, "Failed to get watches");


            var watchesDto = watches.Select(watch => new WatchDto
            {
                Id = watch.Id,
                CreatedAt = watch.CreatedAt,
                UpdatedAt = watch.UpdatedAt,
                Message = watch.Message,
                ExternalLink = watch.ExternalLink,
                ExpirationDate = watch.ExpirationDate,
                CircleId = watch.CircleId,
                Circle = new CircleDto
                {
                    Id = watch.Circle.Id,
                    Name = watch.Circle.Name,
                },
                CreatorId = watch.CreatorId,
                Creator = new UserDto
                {
                    Id = watch.Creator.Id,
                    Username = watch.Creator.Username,
                    UserAvatarUrl = watch.Creator.Avatar != null ? $"/api/download/{watch.Creator.Avatar.FileName}" : null
                },
                Comments = watch.Comments
                .OrderByDescending(comment => comment.CreatedAt)
                .Select(comment => new CommentDto
                {
                    CommentId = comment.Id,
                    CommenterId = comment.CommenterId,
                    CommenterName = comment.Commenter.Username,
                    CommenterAvatarUrl = comment.Commenter.Avatar != null ? $"/api/download/{comment.Commenter.Avatar.FileName}" : null,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    WatchId = comment.WatchId,
                }).ToList()
            }).ToList();


            return new ServiceResponse<List<WatchDto>>(true, watchesDto);
        }
    }
}
