using System;

namespace Tochas.FuzzyLogic.Expressions
{
    public class FuzzyOr : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression OperandA { get; private set; }
        public IFuzzyExpression OperandB { get; private set; }

        public FuzzyOr(IFuzzyExpression operandA, IFuzzyExpression operandB)
        {
            this.OperandA = operandA;
            this.OperandB = operandB;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var valA = this.OperandA.Evaluate(set);
            var valB = this.OperandB.Evaluate(set);
            return MathFz.Clamp01(Math.Max(valA, valB));
        }

        public override string ToString()
        {
            return string.Format("({0} OR {1})", this.OperandA, this.OperandB);
        }
    }
}
