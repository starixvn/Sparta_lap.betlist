namespace Fanex.Athena.UnitTest.Common.Extensions
{
    using Constraints;
    using NUnit.Framework.Constraints;

    public static class CustomConstraintExtensions
    {
        public static SamePropertiesConstraint SameProperties(this ConstraintExpression expression, object expected)
        {
            var constraint = new SamePropertiesConstraint(expected);
            expression.Append(constraint);
            return constraint;
        }
    }
}