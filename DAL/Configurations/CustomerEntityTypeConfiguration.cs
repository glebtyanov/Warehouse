using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityTypeConfigurations
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                .HasMaxLength(50);

            builder.Property(c => c.Address)
                .HasMaxLength(100);

            builder.Property(c => c.ContactNumber)
                .HasMaxLength(20);

            builder.Property(c => c.Email)
                .HasMaxLength(50);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);
        }
    }
}
