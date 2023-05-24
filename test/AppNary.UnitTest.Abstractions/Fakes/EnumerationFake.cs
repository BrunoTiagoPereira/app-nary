using AppNary.Core.ValueObjects;

namespace AppNary.UnitTest.Abstractions.Fakes
{
    public class EnumerationFake<T> : Enumeration<T>
    {
        public EnumerationFake(string name, T value) : base(name, value)
        {
        }
    }
}