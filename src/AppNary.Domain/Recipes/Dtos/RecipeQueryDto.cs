namespace AppNary.Domain.Recipes.Dtos
{
    public class RecipeQueryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LikesCount { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<RecipeIngredientQueryDto> Ingredients { get; set; }


    }
}