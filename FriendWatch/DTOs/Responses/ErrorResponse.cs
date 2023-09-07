namespace FriendWatch.DTOs.Responses
{
    public class ErrorResponse
    {
        public string[] Messages { get; set; } = Array.Empty<string>();

        public ErrorResponse(params string[] messages)
        {
            Messages = messages;
        }
    }
}
