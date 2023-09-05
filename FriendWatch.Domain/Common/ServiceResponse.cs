namespace FriendWatch.Domain.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; }
        public bool IsSuccess { get; }

        public string? Message { get; }

        public ServiceResponse(bool isSuccess, T? data = default, string? message = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }
    }
}
