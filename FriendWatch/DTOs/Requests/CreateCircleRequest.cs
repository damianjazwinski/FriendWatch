namespace FriendWatch.DTOs.Requests
{
    public record CreateCircleRequest
    {
        public string Name { get; set; }

        public IFormFile? Image { get; set; }
    }
}
