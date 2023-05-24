namespace AppNary.Domain.Recipes.Dtos
{
    public class IngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public string SvgIcon { get; set; }
    }
}