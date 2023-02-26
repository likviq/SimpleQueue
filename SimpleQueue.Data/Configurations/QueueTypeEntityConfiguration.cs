using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class QueueTypeEntityConfiguration : IEntityTypeConfiguration<QueueType>
    {
        public void Configure(EntityTypeBuilder<QueueType> builder)
        {
            builder
                .Property(pt => pt.Id)
                .HasColumnName("QueueTypeId")
                .IsRequired();

            builder
                .HasData(
                new QueueType
                {
                    Id = Guid.NewGuid(),
                    Name = TypeName.Fast
                },
                new QueueType
                {
                    Id = Guid.NewGuid(),
                    Name = TypeName.Delayed
                });
        }
    }
}
