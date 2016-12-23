using System;

namespace Tochas.FuzzyLogic
{
    public class FuzzyRule<T> where T : struct, IConvertible
    {
        public T OutputLinguisticVariable { get; private set; }
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyRule(T outputVar, IFuzzyExpression exp)
        {
            FuzzyUtils.IsGenericParameterValid<T>();
            this.OutputLinguisticVariable = outputVar;
            this.Expression = exp;
        }

        public FuzzyValue<T> Evaluate(FuzzyValueSet set)
        {
            FuzzyValue<T> ret = new FuzzyValue<T>();
            ret.linguisticVariable = this.OutputLinguisticVariable;
            ret.membershipDegree = MathFz.Clamp01(this.Expression.Evaluate(set));
            return ret;
        }

        public override string ToString()
        {
            return string.Format("IF {0} THEN {1}", this.Expression, this.OutputLinguisticVariable);
        }
    }
}
