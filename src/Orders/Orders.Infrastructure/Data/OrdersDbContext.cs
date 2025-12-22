using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OutboxMessage> Messages => Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.OrderId)
                    .IsRequired();

                entity.Property(o => o.UserId)
                    .IsRequired();

                entity.Property(o => o.Amount)
                    .IsRequired();

                entity.Property(o => o.Description);

                entity.Property(w => w.Status)
                    .HasConversion<int>()
                    .IsRequired();

                // Индексы
                entity.HasIndex(w => w.UserId);
            });
            
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(o => o.MessageId);

                entity.Property(o => o.MessageId)
                    .IsRequired();

                entity.Property(o => o.Type)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(o => o.Payload)
                    .IsRequired();

                entity.Property(w => w.OccurredAtUtc)
                    .IsRequired();

                entity.Property(e => e.ProcessedAtUtc);
                
                entity.Property(e => e.RetryCount);

                // Индексы
                entity.HasIndex(w => w.Type);
            });
        }
    }
}
