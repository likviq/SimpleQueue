using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class TagEntityConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .Property(pt => pt.Id)
                .HasColumnName("TagId");

            builder
                .Property(pt => pt.TagTitle)
                .HasColumnType("varchar")
                .HasMaxLength(32)
                .IsRequired();
        }
    }
}
