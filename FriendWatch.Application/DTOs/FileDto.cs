namespace FriendWatch.Application.DTOs
{
    public record ImageFileDto
    {
        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public string? Path { get; set; }

        public byte[]? Data { get; set; }

        public string? Url { get; set; }
    }
}
