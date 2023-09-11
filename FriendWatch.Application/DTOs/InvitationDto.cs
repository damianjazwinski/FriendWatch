using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Application.DTOs
{
    public record InvitationDto
    {
        public int CircleId { get; set; }
        public int? InvitationId { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Message { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverUsername { get; set; }
        public string? InvitationCircleName { get; set; }
        public int? InvitationCircleOwnerId { get; set; }
        public string? InvitationCircleOwnerUsername { get; set; }
    }
}
