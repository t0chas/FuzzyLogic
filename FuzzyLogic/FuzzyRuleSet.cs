using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tochas.FuzzyLogic
{

    public interface IRuleEvaluator<T> where T : struct, IConvertible
    {
        FuzzyValue<T>[] EvaluateRules(List<FuzzyRule<T>> rules, FuzzyValueSet inputValueSet);
    }

    public class RuleEvaluator<T> : IRuleEvaluator<T> where T : struct, IConvertible
    {
        private FuzzyValue<T>[] ruleOutputs;

        private void ClearOutputs()
        {
            if (this.ruleOutputs == null)
                return;
            for(int i=0; i< this.ruleOutputs.Length; i++)
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

    public interface IFuzzyValuesMerger<T> where T:struct, IConvertible
    {
        void MergeValues(FuzzyValue<T>[] values, FuzzyValueSet mergedOutputs);
    }

    public class CachedOutputsFuzzyValuesMerger<T> : IFuzzyValuesMerger<T> where T : struct, IConvertible
    {
        private T[] outputEnumValues;
        
        private Dictionary<T, List<FuzzyValue<T>>> duplicateOutputs;

        public CachedOutputsFuzzyValuesMerger()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            this.duplicateOutputs = new Dictionary<T, List<FuzzyValue<T>>>();
            this.outputEnumValues = FuzzyUtils.GetEnumValues<T>();
            for (int i = 0; i < this.outputEnumValues.Length; i++)
            {
                this.duplicateOutputs.Add(this.outputEnumValues[i], new List<FuzzyValue<T>>(10));
            }
        }

        private void ClearOutputs()
        {
            List<FuzzyValue<T>> duplicateList = null;
            for (int i = 0; i < this.outputEnumValues.Length; i++)
            {
                duplicateList = this.duplicateOutputs[this.outputEnumValues[i]];
                duplicateList.Clear();
            }
        }

        private void CollapseOutputs(FuzzyValue<T>[] values)
        {
            List<FuzzyValue<T>> duplicateList = null;
            for (int i = 0; i < values.Length; i++)
            {
                var outputValue = values[i];
                if (outputValue.membershipDegree <= 0.0f)
                {
                    continue;
                }
                duplicateList = this.duplicateOutputs[outputValue.linguisticVariable];
                duplicateList.Add(outputValue);
            }
        }

        public void MergeValues(FuzzyValue<T>[] values, FuzzyValueSet mergedOutputs)
        {
            this.CollapseOutputs(values);
            float maxValue = 0.0f;
            FuzzyValue<T> value;
            List<FuzzyValue<T>> duplicateList = null;
            for (int i = 0; i < this.outputEnumValues.Length; i++)
            {
                maxValue = 0.0f;
                duplicateList = this.duplicateOutputs[this.outputEnumValues[i]];
                for (int j = 0; j < duplicateList.Count; j++)
                {
                    value = duplicateList[j];
                    if (value.membershipDegree > maxValue) //Or-ing outputs
                    {
                        maxValue = value.membershipDegree;
                    }
                }
                mergedOutputs.SetValue(new FuzzyValue<T>(this.outputEnumValues[i], maxValue));
                duplicateList.Clear();
            }
        }
    }

    public interface IDefuzzer<T> where T : struct, IConvertible
    {
        float Defuzze(FuzzySet<T> outputVariableSet, FuzzyValueSet fuzzyValues);
    }

    public class MaxAvDefuzzer<T> : IDefuzzer<T> where T: struct, IConvertible
    {
        private T[] outputEnumValues;

        public MaxAvDefuzzer()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            this.outputEnumValues = FuzzyUtils.GetEnumValues<T>();
        }

        public float Defuzze(FuzzySet<T> outputVariableSet, FuzzyValueSet fuzzyValues)
        {
            float sumRepValConf = 0.0f;
            float sumConf = 0.0f;
            FuzzyVariable<T> fuzzyVar = null;
            FuzzyValue<T> value;
            for (int i = 0; i < this.outputEnumValues.Length; i++)
            {
                T linguisticVar = this.outputEnumValues[i];
                value = fuzzyValues.GetValue(linguisticVar);
                if (value.Confidence <= 0.0f)
                    continue;
                fuzzyVar = outputVariableSet.Get(linguisticVar);
                sumRepValConf += (fuzzyVar.MembershipFunction.RepresentativeValue * value.Confidence);
                sumConf += value.Confidence;
            }
            return sumRepValConf / sumConf;
        }
    }
}
