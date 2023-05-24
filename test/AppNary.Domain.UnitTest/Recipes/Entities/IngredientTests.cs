using AppNary.Core.Exceptions;
using AppNary.Domain.Recipes.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Entities
{
    public class IngredientTests
    {
        private readonly Faker _faker;
        private readonly IngredientFaker _ingredientFaker;
        private readonly Ingredient _ingredient;

        public IngredientTests()
        {
            _faker = new Faker();
            _ingredientFaker = new IngredientFaker();
            _ingredient = _ingredientFaker.Generate();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_exception_when_creating_with_invalid_name(string name)
        {
            FluentActions.Invoking(() => new Ingredient(name, _ingredient.UnitOfMeasure, _ingredient.SvgIcon)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_name_exceeds_max_length()
        {
            FluentActions.Invoking(() => new Ingredient(_faker.Lorem.Letter(Ingredient.MAX_NAME_LENGTH + 1), _ingredient.UnitOfMeasure, _ingredient.SvgIcon)).Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_exception_when_creating_with_invalid_unit_of_measure(string unitOfMeasure)
        {
            FluentActions.Invoking(() => new Ingredient(_ingredient.Name, unitOfMeasure, _ingredient.SvgIcon)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_unit_of_measure_exceeds_max_length()
        {
            FluentActions.Invoking(() => new Ingredient(_ingredient.Name, _faker.Lorem.Letter(Ingredient.MAX_UNIT_OF_MEASURE_LENGTH + 1), _ingredient.SvgIcon)).Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_exception_when_creating_with_invalid_svg_icon(string svgIcon)
        {
            FluentActions.Invoking(() => new Ingredient(_ingredient.Name, _ingredient.UnitOfMeasure, svgIcon)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given / When
            var entity = new Ingredient(_ingredient.Name, _ingredient.UnitOfMeasure, _ingredient.SvgIcon);

            // Then
            entity.Name.Should().Be(_ingredient.Name);
            entity.UnitOfMeasure.Should().Be(_ingredient.UnitOfMeasure);
            entity.SvgIcon.Should().Be(_ingredient.SvgIcon);
        }
    }
}