using AppNary.Core.DomainObjects;
using AppNary.Core.Exceptions;
using AppNary.Domain.Users.Contracts;
using AppNary.Domain.Users.Entities;

namespace AppNary.Domain.Recipes.Entities
{
    public class Recipe : AggregateRoot, IUserRelated
    {
        public const int MAX_NAME_LENGTH = 128;
        public const int MAX_DESCRIPTION_LENGTH = 4096;
        public const int MAX_IMAGE_URL_LENGTH = 2048;
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public string? ImageUrl { get; private set; }
        public bool HasImage => ImageUrl is not null;

        private List<RecipeIngredient> _ingredients;
        public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();

        private List<Like> _likes;
        public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

        public bool CanShow { get; private set; }

        public Recipe(string name, string description, User user, string imageUrl) : this()
        {
            UpdateName(name);
            UpdateDescription(description);
            UpdateUser(user);
            UpdateImageUrl(imageUrl);
            CanShow = false;
        }

        public Recipe(string name, string description, User user) : this()
        {
            UpdateName(name);
            UpdateDescription(description);
            UpdateUser(user);
            CanShow = false;
        }

        protected Recipe()
        {
            _ingredients = new List<RecipeIngredient>();
            _likes = new List<Like>();
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length > MAX_NAME_LENGTH)
            {
                throw new DomainException($"O nome não pode ultrapassar {MAX_NAME_LENGTH} caracteres.");
            }

            Name = name;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (description.Length > MAX_NAME_LENGTH)
            {
                throw new DomainException($"A descrição não pode ultrapassar {MAX_DESCRIPTION_LENGTH} caracteres.");
            }

            Description = description;
        }

        public void UpdateUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserId = user.Id;
            User = user;
        }

        public void UpdateImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            if(imageUrl.Length > MAX_IMAGE_URL_LENGTH)
            {
                throw new DomainException($"A url da imagem não pode ultrapassar {MAX_IMAGE_URL_LENGTH} caracteres.");
            }

            ImageUrl = imageUrl;
        }

        public void RemoveImageUrl()
        {
            ImageUrl = null;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _ingredients.Add(new RecipeIngredient(this, ingredient));
        }
        public void ClearIngredients()
        {
            _ingredients.Clear();
        }

        public void AddLike(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!_likes.Any(x => x.UserId == user.Id))
            {
                _likes.Add(new Like(user, this));
            }
        }

        public void RemoveLike(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var like = _likes.FirstOrDefault(x => x.UserId == user.Id);

            if (like is not null)
            {
                _likes.Remove(like);
            }
        }

        public void UpdateCanShow(bool canShow)
        {
            CanShow = canShow;
        }
    }
}