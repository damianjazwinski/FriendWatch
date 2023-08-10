namespace FriendWatch.Services
{
    public class ServiceResponse<T>
    {
        public T? Data { get; }
        public bool IsSuccess { get; }

        public ServiceResponse(bool isSuccess, T? data = default)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }
}
