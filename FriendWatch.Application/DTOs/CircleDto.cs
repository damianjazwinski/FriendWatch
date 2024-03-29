﻿namespace FriendWatch.Application.DTOs
{
    public record CircleDto
    {
        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ImageFileDto? ImageFile { get; set; }

        public List<UserDto>? Members { get; set; } = new List<UserDto>();
        public UserDto CircleOwner { get; set; }
    }
}
