namespace FriendWatch.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Circle> Circles { get; set; }
        public List<Circle> OwnedCircles { get; set; }
        public List<Invitation> ReceivedInvitations { get; set; }
    }
}
