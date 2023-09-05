using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public record GetOwnedCirclesResponse
    {
        public List<CircleDto> OwnedCircles { get; set; } = new List<CircleDto>();
    }
}
