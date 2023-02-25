using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class QueueTagEntityConfiguration : IEntityTypeConfiguration<QueueTag>
    {
        public void Configure(EntityTypeBuilder<QueueTag> builder)
        {
            builder
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.QueueTags)
                .HasForeignKey(pt => pt.TagId);

            builder
                .HasOne(pt => pt.Queue)
                .WithMany(t => t.QueueTags)
                .HasForeignKey(pt => pt.QueueId);

            builder
                .Property(pt => pt.Id)
                .HasColumnName("QueueTagId")
                .IsRequired();
        }
    }
}
