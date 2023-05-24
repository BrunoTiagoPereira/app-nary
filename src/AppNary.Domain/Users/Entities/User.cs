using AppNary.Core.DomainObjects;
using AppNary.Core.ValueObjects;
using AppNary.Domain.Recipes.Entities;

namespace AppNary.Domain.Users.Entities
{
    public class User : AggregateRoot
    {
        public const int MAX_USER_NAME_LENGTH = 128;
        public string UserName { get; private set; }
        public Password Password { get; private set; }

        private readonly List<Recipe> _recipes;
        public IReadOnlyCollection<Recipe> Recipes => _recipes.AsReadOnly();

        private readonly List<Like> _likes;
        public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

        protected User()
        {
            _likes = new List<Like>();
            _recipes = new List<Recipe>();
        }

        public User(string userName, string password) : this()
        {
            UpdateUserName(userName);
            UpdatePassword(password);
        }

        public void UpdateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
        }

        public void UpdatePassword(string password)
        {
            Password = new Password(password);
        }
    }
}