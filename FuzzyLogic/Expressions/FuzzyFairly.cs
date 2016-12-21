using System;

namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyFairly : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyFairly(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = MathFz.Clamp01(this.Expression.Evaluate(set));
            val = (float)Math.Sqrt(val);
            return val;
        }

        public override string ToString()
        {
            return string.Format("FAIRLY({0})", this.Expression);
        }

    }
}
