using System.ComponentModel.DataAnnotations;

namespace FriendWatch.DTOs.Requests
{
    public record CreateCircleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
