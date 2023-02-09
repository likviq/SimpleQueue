using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(pt => pt.Id);

            builder
                .Property(pt => pt.Id)
                .HasColumnName("UserId")
                .IsRequired();

            builder
                .Property(pt => pt.Username)
                .HasColumnType("varchar")
                .HasMaxLength(32)
                .IsRequired();

            builder
                .Property(pt => pt.Email)
                .HasColumnType("varchar")
                .HasMaxLength(128)
                .IsRequired();

            builder
                .Property(pt => pt.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(21)
                .IsRequired(false);

            builder
                .Property(pt => pt.Name)
                .HasColumnType("varchar")
                .HasMaxLength(64)
                .IsRequired(false);

            builder
                .Property(pt => pt.Surname)
                .HasColumnType("varchar")
                .HasMaxLength(64)
                .IsRequired(false);
        }
    }
}
