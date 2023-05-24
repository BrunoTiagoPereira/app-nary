namespace AppNary.Domain.Recipes.Dtos
{
    public class RecipeItemQueryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int LikesCount { get; set; }
        public bool UserHasLiked { get; set; }
    }
}