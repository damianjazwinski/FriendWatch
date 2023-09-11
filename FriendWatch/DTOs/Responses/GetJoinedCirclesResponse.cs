using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public class GetJoinedCirclesResponse
    {
        public List<CircleDto> JoinedCircles { get; set; } = new List<CircleDto>();
    }
}
