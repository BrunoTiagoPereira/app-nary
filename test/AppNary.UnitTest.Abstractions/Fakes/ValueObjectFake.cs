using AppNary.Core.ValueObjects;

namespace AppNary.UnitTest.Abstractions.Fakes
{
    public class ValueObjectFake : ValueObject<string>
    {
        public ValueObjectFake(string value) : base(value)
        {
        }
    }
}