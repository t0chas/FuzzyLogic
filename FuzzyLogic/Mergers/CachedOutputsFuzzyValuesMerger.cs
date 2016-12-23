using System;
using System.Collections.Generic;

namespace Tochas.FuzzyLogic.Mergers
{
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
            FuzzyUtils.IsGenericParameterValid<T>();
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
                mergedOutputs.Set(new FuzzyValue<T>(this.outputEnumValues[i], maxValue));
                duplicateList.Clear();
            }
        }
    }
}
