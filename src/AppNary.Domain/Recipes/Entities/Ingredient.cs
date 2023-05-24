using AppNary.Core.DomainObjects;
using AppNary.Core.Exceptions;

namespace AppNary.Domain.Recipes.Entities
{
    public class Ingredient : Entity
    {
        public const int MAX_NAME_LENGTH = 128;
        public const int MAX_UNIT_OF_MEASURE_LENGTH = 8;

        public Ingredient(string name, string unitOfMeasure, string svgIcon)
        {
            UpdateName(name);
            UpdateUnitOfMeasure(unitOfMeasure);
            UpdateSvgIcon(svgIcon);
        }

        protected Ingredient()
        { }

        public string Name { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public string SvgIcon { get; private set; }

        private void UpdateName(string name)
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

        private void UpdateUnitOfMeasure(string unitOfMeasure)
        {
            if (string.IsNullOrWhiteSpace(unitOfMeasure))
            {
                throw new ArgumentNullException(nameof(unitOfMeasure));
            }

            if (unitOfMeasure.Length > MAX_UNIT_OF_MEASURE_LENGTH)
            {
                throw new DomainException($"A unidade de medida não pode ultrapassar {MAX_UNIT_OF_MEASURE_LENGTH} caracteres.");
            }

            UnitOfMeasure = unitOfMeasure;
        }

        private void UpdateSvgIcon(string svgIcon)
        {
            if (string.IsNullOrWhiteSpace(svgIcon))
            {
                throw new ArgumentNullException(nameof(svgIcon));
            }

            SvgIcon = svgIcon;
        }
    }
}