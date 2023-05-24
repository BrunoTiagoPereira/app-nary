using AppNary.Domain.Recipes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppNary.Data.Mappers
{
    public class RecipeMapper : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Ignore(x => x.Events);

            builder.Property(x => x.Name).HasMaxLength(Recipe.MAX_NAME_LENGTH).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(Recipe.MAX_DESCRIPTION_LENGTH).IsRequired();

            builder.Property(x => x.ImageUrl).HasMaxLength(Recipe.MAX_IMAGE_URL_LENGTH);

            builder.Ignore(x => x.HasImage);

            builder.Property(x => x.CanShow).IsRequired();

            builder.HasMany(x => x.Ingredients).WithOne(x => x.Recipe);

            builder.HasMany(x => x.Likes).WithOne(x => x.Recipe);
        }
    }
}