using AdgBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdgBugTracker.Data
{
    public class AdgBugTrackerDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public AdgBugTrackerDbContext(DbContextOptions<AdgBugTrackerDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("Developer"));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Avoid cycles in Bugs table
            builder.Entity<Bug>()
                .HasOne(e => e.ClosedUser)
                .WithMany()
                .HasForeignKey(e => e.ClosedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bug>()
                .HasOne(e => e.ReopenedUser)
                .WithMany()
                .HasForeignKey(e => e.ReopenedById)
                .OnDelete(DeleteBehavior.Restrict);
            //Avoid cycles in Notes tables
            builder.Entity<Notes>()
                .HasOne(e => e.CreatedUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notes>()
                .HasOne(e => e.UpdatedUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            //Avoid cycles in Projects table
            //builder.Entity<Project>()
            //    .HasOne(e => e.User)
            //    .WithMany()
            //    .HasForeignKey(e => e.CreatedBy)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
    }
}
