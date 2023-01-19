using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class UserInQueueEntityConfiguration : IEntityTypeConfiguration<UserInQueue>
    {
        public void Configure(EntityTypeBuilder<UserInQueue> builder)
        {
            builder
                .HasOne(pt => pt.User)
                .WithMany(t => t.UserInQueues)
                .HasForeignKey(pt => pt.UserId);

            builder
                .HasOne(pt => pt.Queue)
                .WithMany(t => t.UserInQueues)
                .HasForeignKey(pt => pt.QueueId);

            builder
                .Property(pt => pt.Id)
                .HasColumnName("UserInQueueId")
                .IsRequired();

            builder
                .Property(pt => pt.JoinTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .Property(pt => pt.NextId)
                .HasDefaultValue(null);
        }
    }
}
