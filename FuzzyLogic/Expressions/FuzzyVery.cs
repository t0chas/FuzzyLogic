namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyVery : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyVery(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = MathFz.Clamp01(this.Expression.Evaluate(set));
            val = val * val;
            return val;
        }

        public override string ToString()
        {
            return string.Format("VERY({0})", this.Expression);
        }

    }
}
