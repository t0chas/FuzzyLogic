using System;

namespace Tochas.FuzzyLogic.Defuzzers
{
    public class MaxAvDefuzzer<T> : IDefuzzer<T> where T : struct, IConvertible
    {
        private T[] outputEnumValues;

        public MaxAvDefuzzer()
        {
            FuzzyUtils.IsGenericParameterValid<T>();
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
                value = fuzzyValues.Get(linguisticVar);
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
