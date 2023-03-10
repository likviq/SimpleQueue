using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class QueueEntityConfiguration : IEntityTypeConfiguration<Queue>
    {
        public void Configure(EntityTypeBuilder<Queue> builder)
        {
            builder
                .HasKey(pt => pt.Id);

            builder
                .Property(pt => pt.Id)
                .HasColumnName("QueueId")
                .IsRequired();

            builder
                .Property(pt => pt.Title)
                .HasColumnType("varchar")
                .HasMaxLength(32)
                .IsRequired();

            builder
                .Property(pt => pt.Latitude)
                .IsRequired(false);

            builder
                .Property(pt => pt.Longitude)
                .IsRequired(false);

            builder
                .Property(pt => pt.Password)
                .IsRequired(false);

            builder
                .Property(pt => pt.Chat)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(pt => pt.CreatedTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(pt => pt.StartTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .HasOne<QueueType>(s => s.QueueType)
                .WithMany(g => g.Queues)
                .HasForeignKey(s => s.QueueTypeId);

            builder
                .HasOne<ImageBlob>(ad => ad.ImageBlob)
                .WithOne(s => s.Queue)
                .HasForeignKey<Queue>(ad => ad.ImageBlobId);
        }
    }
}
