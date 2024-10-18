using AdgBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdgBugTracker.Data
{
    public class AdgBugTrackerDbContext : IdentityDbContext<IdentityUser>
    {
        public AdgBugTrackerDbContext(DbContextOptions<AdgBugTrackerDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
    }
}
