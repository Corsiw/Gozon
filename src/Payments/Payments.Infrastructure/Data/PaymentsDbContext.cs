using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Events;

namespace Infrastructure.Data
{
    public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
    {
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(o => o.BankAccountId);

                entity.Property(o => o.BankAccountId)
                    .IsRequired();

                entity.Property(o => o.UserId)
                    .IsRequired();

                entity.Property(o => o.Amount)
                    .IsRequired();

                entity.Property(o => o.Version)
                    .IsRequired()
                    .IsConcurrencyToken();

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
            
            modelBuilder.Entity<InboxMessage>(entity =>
            {
                entity.HasKey(o => o.MessageId);

                entity.Property(o => o.MessageId)
                    .IsRequired();

                entity.Property(e => e.ProcessedAtUtc);
            });
        }
    }
}