namespace Fanex.Athena.UnitTest.Common.Extensions
{
    using Fanex.Athena.UnitTest.Common.Constraints;

    public class Tobe : NUnit.Framework.Is
    {
        public static SamePropertiesConstraint SameProperties(object expected)
        {
            return new SamePropertiesConstraint(expected);
        }
    }
}