using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanAuth.Domain.Entities;

namespace CleanAuth.Infrastructure.Common.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).HasMaxLength(100);
            builder.Property(u => u.Email).HasMaxLength(255);
        }
    }
}
