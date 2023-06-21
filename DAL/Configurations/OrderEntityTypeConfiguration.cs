using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            builder.HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusId);

            builder.HasOne(o => o.Transaction)
                .WithOne(t => t.Order)
                .HasForeignKey<Transaction>(t => t.OrderId);

            builder.HasOne(o => o.Worker)
                .WithMany(w => w.Orders)
                .HasForeignKey(o => o.WorkerId);

            builder.HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<OrderProduct>(
                    j => j.HasOne(op => op.Product)
                        .WithMany()
                        .HasForeignKey(op => op.ProductId),
                    j => j.HasOne(op => op.Order)
                        .WithMany()
                        .HasForeignKey(op => op.OrderId),
                    j =>
                    {
                        j.HasKey(op => new { op.OrderId, op.ProductId });
                        j.ToTable("OrderProducts");
                    }
                );
        }
    }
}