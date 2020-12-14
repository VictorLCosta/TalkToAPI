using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalkToAPI.V1.Models;

namespace TalkToAPI.Database
{
    public class TalkToContext : IdentityDbContext<ApplicationUser>
    {
        public TalkToContext(DbContextOptions<TalkToContext> options)
            : base (options)
        {
            
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Message>(op => {
                op.HasKey(m => m.Id);

                op.HasOne(m => m.Sender)
                    .WithMany(s => s.Messages)
                    .HasForeignKey(m => m.SenderId);

                op.HasOne(m => m.To);
            });
        }
    }
}