using Microsoft.EntityFrameworkCore;
using VotingSystemAPI.Data.Models;
using VotingSystemAPI.Data.Models.VotingSystemAPI.Data.Models;

namespace VotingSystemAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Vote> Votes { get; set; }
    }
}