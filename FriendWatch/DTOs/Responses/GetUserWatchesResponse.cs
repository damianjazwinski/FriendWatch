using System.Collections;

using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public record GetUserWatchesResponse
    {
        public List<WatchResponseDto> Watches { get; set; } = new();
    }

}
