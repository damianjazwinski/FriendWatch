using System.Security.Claims;

using FriendWatch.Data.Repositories.CircleRepository;
using FriendWatch.DTOs.Requests;
using FriendWatch.Entities;

namespace FriendWatch.Services.CircleService
{
    public class CircleService : ICircleService
    {
        private readonly ICircleRepository _circleRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CircleService(ICircleRepository circleRepository, IHttpContextAccessor contextAccessor)
        {
            _circleRepository = circleRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task CreateCircleAsync(CreateCircleRequest request)
        {
            var currentUserId = int.Parse(_contextAccessor.HttpContext!.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var circle = new Circle
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = currentUserId
            };

            await _circleRepository.CreateAsync(circle);

            return;
        }
    }
}
