using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.PaymentMethod)
                .HasMaxLength(50);

            builder.HasOne(t => t.Order)
                .WithOne(o => o.Transaction)
                .HasForeignKey<Transaction>(t => t.OrderId);
        }
    }
}