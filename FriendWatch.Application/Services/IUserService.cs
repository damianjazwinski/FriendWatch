﻿using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDto>> CreateUserAsync(UserRequestDto userDto);
        Task<ServiceResponse<UserDto>> GetByIdAsync(int id);
        Task<ServiceResponse<UserDto>> GetByIdAsync(string id);
        Task<ServiceResponse<UserDto>> GetByUsernameAsync(string username);
        Task<ServiceResponse<UserDto>> SetUserAvatar(UserDto userDto);
    }
}
