using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public record GetCircleWithMembers
    {
        public CircleDto Circle { get; set; } = new();
    }
}
