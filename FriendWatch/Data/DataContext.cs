using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conntectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=100;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;";
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(conntectionString);
        }

        public DbSet<User> Users { get; set; }
    }
}
