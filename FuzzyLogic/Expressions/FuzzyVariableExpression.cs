using System;

namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyVariableExpression<T> : IFuzzyExpression where T : struct, IConvertible
    {
        public T LinguisticVariable { get; private set; }

        public FuzzyVariableExpression(T linguisticVar)
        {
            this.LinguisticVariable = linguisticVar;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var fuzzyVar = set.Get(this.LinguisticVariable);
            return MathFz.Clamp01(fuzzyVar.membershipDegree);
        }

        public override string ToString()
        {
            return this.LinguisticVariable.ToString();
        }
    }
}
