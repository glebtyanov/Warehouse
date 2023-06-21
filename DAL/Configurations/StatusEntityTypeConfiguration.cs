using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityTypeConfigurations
{
    public class StatusEntityTypeConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(s => s.StatusId);

            builder.Property(s => s.Name)
                .HasMaxLength(50);

            builder.HasMany(s => s.Orders)
                .WithOne(o => o.Status)
                .HasForeignKey(o => o.StatusId);
        }
    }
}
