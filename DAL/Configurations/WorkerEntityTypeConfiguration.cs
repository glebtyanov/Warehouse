using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class WorkerEntityTypeConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(w => w.WorkerId);

            builder.Property(w => w.FirstName)
                .HasMaxLength(50);

            builder.Property(w => w.LastName)
                .HasMaxLength(50);

            builder.Property(w => w.Address)
                .HasMaxLength(100);

            builder.Property(w => w.ContactNumber)
                .HasMaxLength(20);

            builder.Property(w => w.Email)
                .HasMaxLength(50);

            builder.HasMany(w => w.Orders)
                .WithOne(o => o.Worker);

            builder.HasOne(w => w.Position)
                .WithMany(p => p.Workers);
        }
    }
}