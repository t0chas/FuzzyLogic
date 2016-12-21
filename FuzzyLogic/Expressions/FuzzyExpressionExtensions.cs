using System;

namespace Tochas.FuzzyLogic.Expressions
{
    public static class FuzzyExpressionExtensions
    {
        public static IFuzzyExpression Expr<T>(this T enumValue) where T : struct, IConvertible
        {
            return new FuzzyVariableExpression<T>(enumValue);
        }

        public static IFuzzyExpression And(this IFuzzyExpression exp, IFuzzyExpression other)
        {
            FuzzyAnd and = new FuzzyAnd(exp, other);
            return and;
        }

        public static IFuzzyExpression And<T>(this IFuzzyExpression exp, FuzzyVariable<T> fuzzVar) where T : struct, IConvertible
        {
            return exp.And(fuzzVar.AsExpression());
        }

        public static IFuzzyExpression Or(this IFuzzyExpression exp, IFuzzyExpression other)
        {
            FuzzyOr or = new FuzzyOr(exp, other);
            return or;
        }

        public static IFuzzyExpression Not(this IFuzzyExpression exp)
        {
            FuzzyNot not = new FuzzyNot(exp);
            return not;
        }

        public static FuzzyRule<T> Then<T>(this IFuzzyExpression exp, T outputLinguisticVar) where T : struct, IConvertible
        {
            FuzzyRule<T> rule = new FuzzyRule<T>(outputLinguisticVar, exp);
            return rule;
        }
    }
}
