using AppNary.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppNary.Data.Mappers
{
    public class UserMapper : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Ignore(x => x.Events);

            builder.Property(x => x.UserName).HasMaxLength(User.MAX_USER_NAME_LENGTH).IsRequired();

            builder.OwnsOne(x => x.Password).Property(x => x.Hash).HasMaxLength(150).IsRequired();

            builder.HasMany(x => x.Likes).WithOne(x => x.User).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Recipes).WithOne(x => x.User);
        }
    }
}