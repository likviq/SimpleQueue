using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class ConversationEntityConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder
                .Property(pt => pt.Id)
                .HasColumnName("ConversationId");

            builder
                .Property(pt => pt.Time)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
