using System;

namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyAnd : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression OperandA { get; private set; }
        public IFuzzyExpression OperandB { get; private set; }

        public FuzzyAnd(IFuzzyExpression operandA, IFuzzyExpression operandB)
        {
            this.OperandA = operandA;
            this.OperandB = operandB;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var valA = this.OperandA.Evaluate(set);
            var valB = this.OperandB.Evaluate(set);
            return MathFz.Clamp01(Math.Min(valA, valB));
        }

        public override string ToString()
        {
            return string.Format("({0} AND {1})", this.OperandA, this.OperandB);
        }
    }
}
