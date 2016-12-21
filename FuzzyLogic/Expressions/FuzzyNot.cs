namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyNot : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyNot(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = MathFz.Clamp01(this.Expression.Evaluate(set));
            float ret = 1.0f - val;
            return ret;
        }

        public override string ToString()
        {
            return string.Format("NOT({0})", this.Expression);
        }

    }
}
