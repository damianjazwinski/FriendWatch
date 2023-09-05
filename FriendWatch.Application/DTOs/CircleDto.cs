using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Application.DTOs
{
    public record CircleDto
    {
        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public byte[]? Image { get; set; }

        public string? Extension { get; set; }

        public string? ImagePath { get; set; }
    }
}
