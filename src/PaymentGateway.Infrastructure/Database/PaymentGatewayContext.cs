using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain;
using System.Linq;

namespace PaymentGateway.Infrastructure.Database
{
    public class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("checkout");

            modelBuilder.Entity<Payment>().HasKey(x => x.Id);
            modelBuilder.Entity<Payment>().Property(x => x.Amount).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.Currency).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.Expiry).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.CardNumber).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.FullName).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.BankPaymentId).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.BankPaymentStatus).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.CreatedAt).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void BeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(x =>
                x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity) entry.Entity).CreatedAt = now;
                }
            }
        }
    }
}