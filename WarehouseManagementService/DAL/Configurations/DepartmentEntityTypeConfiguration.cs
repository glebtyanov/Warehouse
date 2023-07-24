using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityTypeConfigurations
{
    public class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.Name)
                .HasMaxLength(50);

            builder.Property(d => d.Location)
                .HasMaxLength(100);

            builder.HasMany(d => d.Workers)
            .WithMany(w => w.Departments)
            .UsingEntity<DepartmentWorker>(
                j => j.HasOne(dw => dw.Worker)
                    .WithMany()
                    .HasForeignKey(dw => dw.WorkerId),
                j => j.HasOne(dw => dw.Department)
                    .WithMany()
                    .HasForeignKey(dw => dw.DepartmentId),
                j =>
                {
                    j.HasKey(dw => new { dw.DepartmentId, dw.WorkerId });
                    j.ToTable("DepartmentWorkers");
                }
            );
        }
    }
}


