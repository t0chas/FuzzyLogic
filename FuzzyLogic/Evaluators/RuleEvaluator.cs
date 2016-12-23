using System;
using System.Collections.Generic;

namespace Tochas.FuzzyLogic.Evaluators
{
    public class RuleEvaluator<T> : IRuleEvaluator<T> where T : struct, IConvertible
    {
        private FuzzyValue<T>[] ruleOutputs;

        public RuleEvaluator()
        {
            FuzzyUtils.IsGenericParameterValid<T>();
        }

        private void ClearOutputs()
        {
            if (this.ruleOutputs == null)
                return;
            for (int i = 0; i < this.ruleOutputs.Length; i++)
            {
                this.ruleOutputs[i].membershipDegree = 0.0f;
            }
        }

        public FuzzyValue<T>[] EvaluateRules(List<FuzzyRule<T>> rules, FuzzyValueSet inputValueSet)
        {
            int ruleCount = rules.Count;
            if (this.ruleOutputs == null || (this.ruleOutputs != null && this.ruleOutputs.Length < ruleCount))
                this.ruleOutputs = new FuzzyValue<T>[ruleCount];
            else
                this.ClearOutputs();
            for (int i = 0; i < ruleCount; i++)
            {
                this.ruleOutputs[i] = rules[i].Evaluate(inputValueSet);
            }
            return this.ruleOutputs;
        }
    }
}
