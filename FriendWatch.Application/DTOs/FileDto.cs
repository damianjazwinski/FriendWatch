using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
