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
                    Id = new Guid("2550a9b8-55c5-499d-82db-0f151ff291c5"),
                    Name = TypeName.Fast
                },
                new QueueType
                {
                    Id = new Guid("4a0ede84-0d59-4a97-9a82-96d8e386c730"),
                    Name = TypeName.Delayed
                });
        }
    }
}
