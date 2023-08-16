using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;

namespace FriendWatch.Infrastructure.Services
{
    public class CircleService : ICircleService
    {
        private readonly ICircleRepository _circleRepository;

        public CircleService(ICircleRepository circleRepository)
        {
            _circleRepository = circleRepository;
        }

        public async Task CreateCircleAsync(CircleDto circleDto, int currentUserId)
        {
            var circle = new Circle
            {
                Name = circleDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = currentUserId
            };

            await _circleRepository.CreateAsync(circle);

            return;
        }
    }
}
