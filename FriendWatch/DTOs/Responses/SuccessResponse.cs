namespace FriendWatch.DTOs.Responses
{
    public class SuccessResponse
    {
        public string[] Messages { get; set; } = Array.Empty<string>();

        public SuccessResponse(params string[] messages)
        {
            Messages = messages;
        }
    }
}
