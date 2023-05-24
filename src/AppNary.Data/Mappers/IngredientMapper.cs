using AppNary.Domain.Recipes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppNary.Data.Mappers
{
    public class IngredientMapper : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Ignore(x => x.Events);

            builder.Property(x => x.Name).HasMaxLength(Ingredient.MAX_NAME_LENGTH).IsRequired();

            builder.Property(x => x.UnitOfMeasure).IsRequired().HasMaxLength(Ingredient.MAX_UNIT_OF_MEASURE_LENGTH);

            builder.Property(x => x.SvgIcon).IsRequired();
        }
    }
}