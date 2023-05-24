using AppNary.Domain.Recipes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppNary.Data.Mappers
{
    public class RecipeIngredientMapper : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Ignore(x => x.Events);

            builder.HasOne(x => x.Recipe).WithMany(x => x.Ingredients);

            builder.HasOne(x => x.Ingredient).WithMany();
        }
    }
}