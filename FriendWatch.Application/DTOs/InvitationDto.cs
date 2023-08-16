using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Application.DTOs
{
    public class InvitationDto
    {
        public string? Message { get; set; }
        public int UserId { get; set; }
        public int CircleId { get; set; }
    }
}
