using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voting_Test.Models;

namespace Voting_Test.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser>ApplicationUsers { get; set; }
        public DbSet<PollingRoom> PollingRooms{ get; set; }
        public DbSet<Poll> Polls{ get; set; }
        public DbSet<Vote>Votes  { get; set; }
    }
}
