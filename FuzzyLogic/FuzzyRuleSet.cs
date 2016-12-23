using System;
using System.Collections.Generic;
using Tochas.FuzzyLogic.Defuzzers;
using Tochas.FuzzyLogic.Evaluators;
using Tochas.FuzzyLogic.Mergers;

namespace Tochas.FuzzyLogic
{
    public class FuzzyRuleSet<T> where T : struct, IConvertible
    {
        private List<FuzzyRule<T>> rules;
        public FuzzySet<T> OutputVarSet { get; private set; }

        public FuzzyValueSet OutputValueSet { get; private set; }

        public IRuleEvaluator<T> RuleEvaluator { get; set; }
        public IFuzzyValuesMerger<T> OutputsMerger { get; set; }
        public IDefuzzer<T> Defuzzer { get; set; }

        public FuzzyRuleSet(FuzzySet<T> varSet, List<FuzzyRule<T>> rules, IRuleEvaluator<T> ruleEvaluator, IFuzzyValuesMerger<T> outputsMerger, IDefuzzer<T> defuzzer)
        {
            FuzzyUtils.IsGenericParameterValid<T>();
            this.rules = rules;
            this.OutputVarSet = varSet;
            this.OutputValueSet = new FuzzyValueSet();
            this.RuleEvaluator = ruleEvaluator;
            this.OutputsMerger = outputsMerger;
            this.Defuzzer = defuzzer;
        }

        public FuzzyRuleSet(FuzzySet<T> varSet, IEnumerable<FuzzyRule<T>> rules) :
            this(varSet, new List<FuzzyRule<T>>(rules), new RuleEvaluator<T>(), new CachedOutputsFuzzyValuesMerger<T>(), new MaxAvDefuzzer<T>())
        {

        }

        public FuzzyRuleSet() : this(new FuzzySet<T>(), new List<FuzzyRule<T>>())
        {

        }

        public FuzzyRuleSet(FuzzySet<T> varSet) : this(varSet, new List<FuzzyRule<T>>())
        {

        }

        public FuzzyRuleSet(IEnumerable<FuzzyRule<T>> rules) : this(new FuzzySet<T>(), new List<FuzzyRule<T>>(rules))
        {

        }

        public void Add(FuzzyRule<T> rule)
        {
            this.rules.Add(rule);
        }

        public void Add(IEnumerable<FuzzyRule<T>> rules)
        {
            this.rules.AddRange(rules);
        }

        public float Evaluate(FuzzyValueSet inputValueSet)
        {
            if (!this.OutputVarSet.IsValid())
                return float.NaN;
            int ruleCount = this.rules.Count;
            var ruleOutputs = this.RuleEvaluator.EvaluateRules(this.rules, inputValueSet);
            this.OutputsMerger.MergeValues(ruleOutputs, this.OutputValueSet);
            var result = this.Defuzzer.Defuzze(this.OutputVarSet, this.OutputValueSet);
            return result;
        }
    }
}
