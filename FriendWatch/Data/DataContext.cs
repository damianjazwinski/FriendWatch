using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conntectionString = _configuration.GetConnectionString("MSSQL") 
                ?? throw new Exception("Error on reading connection string");

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(conntectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Circles)
                .WithMany(circle => circle.Members)
                .UsingEntity(
                    "CircleUser",
                    l => l.HasOne(typeof(Circle)).WithMany().HasForeignKey("CirclesId").HasPrincipalKey(nameof(Circle.Id)).OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("MembersId").HasPrincipalKey(nameof(User.Id)).OnDelete(DeleteBehavior.NoAction),
                    j => j.HasKey("MembersId", "CirclesId"));

            modelBuilder.Entity<Circle>()
                .HasOne(circle => circle.Owner)
                .WithMany(owner => owner.OwnedCircles)
                .HasForeignKey(circle => circle.OwnerId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Circle> Circles { get; set; }
    }
}
