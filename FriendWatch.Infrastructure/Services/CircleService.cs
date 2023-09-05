using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Infrastructure.Services
{
    public class CircleService : ICircleService
    {
        private readonly ICircleRepository _circleRepository;
        private readonly IUserRepository _userRepository;

        public CircleService(ICircleRepository circleRepository, IUserRepository userRepository)
        {
            _circleRepository = circleRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<CircleDto>> CreateCircleAsync(CircleDto circleDto, int currentUserId)
        {
            var existingCircle = await _circleRepository.GetByOwnerIdAndNameAsync(currentUserId, circleDto.Name);

            if (existingCircle != null)
                return new ServiceResponse<CircleDto>(false, null, "Circle name already used");

            var circle = new Circle
            {
                Name = circleDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = currentUserId
            };

            await _circleRepository.CreateAsync(circle);

            if(circleDto.Image != null)
            {
                var directoryInfo = Directory.CreateDirectory(@"files\circles");
                var fullPathWithName = Path.Combine(directoryInfo.ToString(), $"{circle.Id}{circleDto.Extension}");
                using (var stream = File.Create(fullPathWithName))
                {
                    await stream.WriteAsync(circleDto.Image, 0, circleDto.Image.Length);
                }
            }
            
            return new ServiceResponse<CircleDto>(true, null);
        }

        public async Task<ServiceResponse<List<CircleDto>>> GetJoinedCirclesAsync(int currentUserId)
        {
            var user = await _userRepository.GetByIdAsync(currentUserId);
            var joinedCircles = user.Circles;
            var joinedCirclesDto = joinedCircles.Select(circle => new CircleDto { Name = circle.Name }).ToList();

            return new ServiceResponse<List<CircleDto>>(true, joinedCirclesDto);
        }

        public async Task<ServiceResponse<List<CircleDto>>> GetOwnedCirclesAsync(int currentUserId)
        {
            var user = await _userRepository.GetByIdAsync(currentUserId);
            var ownedCircles = user.OwnedCircles;
            var ownedCirclesDto = ownedCircles.Select(circle => new CircleDto { Id = circle.Id, Name = circle.Name, ImagePath = $"files/circles/{circle.Id}.jpg" }).ToList(); // TODO: Don't hardcode extension!!!

            return new ServiceResponse<List<CircleDto>>(true, ownedCirclesDto);
        }
    }
}
