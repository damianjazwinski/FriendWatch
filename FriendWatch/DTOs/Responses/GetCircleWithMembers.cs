using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Entities;

namespace FriendWatch.DTOs.Responses
{
    public record GetCircleWithMembers
    {
        public CircleDto Circle { get; set; } = new();
    }
}
