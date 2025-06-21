using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CreativeDesk.Models; 

namespace CreativeDesk.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<ProjectDesigner> ProjectDesigners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            
            modelBuilder.Entity<ProjectDesigner>()
                .HasKey(pd => new { pd.ProjectId, pd.DesignerId });

            modelBuilder.Entity<ProjectDesigner>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.ProjectDesigners)
                .HasForeignKey(pd => pd.ProjectId);

            modelBuilder.Entity<ProjectDesigner>()
                .HasOne(pd => pd.Designer)
                .WithMany(d => d.ProjectDesigns)
                .HasForeignKey(pd => pd.DesignerId);
        }
    }
}
