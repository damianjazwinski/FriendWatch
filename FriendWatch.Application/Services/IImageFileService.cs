using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IImageFileService
    {
        Task<ServiceResponse<ImageFileDto>> GetFileByNameAsync(string name);
    }
}
