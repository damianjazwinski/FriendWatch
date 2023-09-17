using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FriendWatch.Infrastructure.Services
{
    public class CircleService : ICircleService
    {
        private readonly ICircleRepository _circleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageFileRepository _imageFileRepository;

        public CircleService(ICircleRepository circleRepository, IUserRepository userRepository,
            IImageFileRepository imageFileRepository)
        {
            _circleRepository = circleRepository;
            _userRepository = userRepository;
            _imageFileRepository = imageFileRepository;
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


            if (circleDto.ImageFile?.Data != null)
            {
                var fileExtension = Path.GetExtension(circleDto.ImageFile.FileName);
                var directoryInfo = Directory.CreateDirectory(@"files\circles");
                var generatedFileName = $"{Path.ChangeExtension(Path.GetRandomFileName(), fileExtension)}";
                var fullPathWithName = Path.Combine(directoryInfo.ToString(), generatedFileName);

                circleDto.ImageFile.Path = fullPathWithName;
                circleDto.ImageFile.FileName = generatedFileName;

                using (var stream = File.Create(fullPathWithName))
                {
                    await stream.WriteAsync(circleDto.ImageFile.Data, 0, circleDto.ImageFile.Data.Length);
                }

                circle.ImageFile = new ImageFile
                {
                    FileName = generatedFileName,
                    ContentType = circleDto.ImageFile.ContentType!,
                    Path = Path.Combine("files\\circles", generatedFileName)
                };
            };

            var user = await _userRepository.GetByIdAsync(currentUserId);

            circle.Members = new List<User> { user };

            await _circleRepository.CreateAsync(circle);

            return new ServiceResponse<CircleDto>(true, null);
        }

        public async Task<ServiceResponse<CircleDto>> GetByIdWithMembersAsync(int id, int currentUserId)
        {
            var circle = await _circleRepository.GetByIdAsync(id);

            if (circle == null)
                return new ServiceResponse<CircleDto>(false, null, "Circle not found");

            var circleDto = new CircleDto
            {
                Id = circle.Id,
                ImageFile = circle.ImageFile != null ? new ImageFileDto { Url = $"/api/download/{circle.ImageFile.FileName}" } : null,
                Name = circle.Name,
                Members = circle.Members.Select(member => new UserDto { Id = member.Id, Username = member.Username }).ToList()
            };

            return new ServiceResponse<CircleDto>(true, circleDto);
        }

        public async Task<ServiceResponse<List<CircleDto>>> GetJoinedCirclesAsync(int currentUserId)
        {
            var joinedCircles = await _circleRepository.GetJoinedAsync(currentUserId);

            // remove owned circle

            var joinedCirclesDto = joinedCircles
                .Where(circle => circle.OwnerId != currentUserId)
                .Select(circle => new CircleDto
                {
                    Id = circle.Id,
                    CircleOwner = new UserDto 
                    { 
                        Id = circle.OwnerId, 
                        Username = circle.Owner.Username 
                    },
                    Name = circle.Name,
                    ImageFile = circle.ImageFile != null ? new ImageFileDto { Url = $"/api/download/{circle.ImageFile.FileName}" } : null
                }).ToList();

            return new ServiceResponse<List<CircleDto>>(true, joinedCirclesDto);
        }

        public async Task<ServiceResponse<List<CircleDto>>> GetOwnedCirclesAsync(int currentUserId)
        {
            var ownedCircles = await _circleRepository.GetByOwnerIdAsync(currentUserId);
            var ownedCirclesDto = ownedCircles.Select(circle =>
            new CircleDto
            {
                Id = circle.Id,
                Name = circle.Name,
                ImageFile = circle.ImageFile != null ? new ImageFileDto { Url = $"/api/download/{circle.ImageFile.FileName}" } : null
            }).ToList();

            return new ServiceResponse<List<CircleDto>>(true, ownedCirclesDto);
        }
    }
}
