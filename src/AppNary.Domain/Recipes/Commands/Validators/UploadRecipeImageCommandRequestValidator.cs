using FluentValidation;
using System.Collections.Immutable;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class UploadRecipeImageCommandRequestValidator : AbstractValidator<UploadRecipeImageCommandRequest>
    {
        public const int MAX_FILE_SIZE_IN_BYTES = 2048 * 1000;

        public UploadRecipeImageCommandRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("A receita é inválida.");

            When(x => x.Image is not null, () =>
            {
                RuleFor(x => x.Image.FileName).NotEmpty().WithMessage("A imagem não é válida");

                RuleFor(x => x.Image.Length).GreaterThan(0).WithMessage("A imagem não é válida");

                RuleFor(x => x.Image.Length).LessThan(MAX_FILE_SIZE_IN_BYTES).WithMessage($"A imagem deve ter no máximo {MAX_FILE_SIZE_IN_BYTES / 1000}kb");

                When(x => !string.IsNullOrWhiteSpace(x.Image.FileName), () =>
                {
                    RuleFor(x => x.Image.FileName).Must(HasValidExtension).WithMessage("A imagem deve ter uma extensão png");
                });
            });
        }

        private bool HasValidExtension(string fileName)
        {
            if (!Path.HasExtension(fileName)) return false;

            var validExtensions = new[] { ".png" };
            var extension = Path.GetExtension(fileName).ToLower();

            return validExtensions.Contains(extension);
        }
    }
}