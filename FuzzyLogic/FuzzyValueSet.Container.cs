using System.Collections.Generic;
using Tochas.FuzzyLogic.Utils;

namespace Tochas.FuzzyLogic
{
    public partial class FuzzyValueSet
    {
        private class Container
        {
            
            private Dictionary<EnumKey, float> _internalDictionary;
            
            internal Container(int size = 100)
            {
                this._internalDictionary = new Dictionary<EnumKey, float>(size);
            }

            public void Set<T>(FuzzyValue<T> fuzzyValue) where T : struct, System.IConvertible
            {
                FuzzyUtils.IsGenericParameterValid<T>();
                EnumKey key = EnumKey.From(fuzzyValue.linguisticVariable);
                float degree = fuzzyValue.membershipDegree;
                this._internalDictionary[key] = degree;
            }

            public FuzzyValue<T> Get<T>(T linguisticVariable) where T : struct, System.IConvertible
            {
                FuzzyUtils.IsGenericParameterValid<T>();
                EnumKey key = EnumKey.From(linguisticVariable);
                float degree = 0.0f;
                this._internalDictionary.TryGetValue(key, out degree);
                FuzzyValue<T> fuzzyValue = new FuzzyValue<T>(linguisticVariable, degree);
                return fuzzyValue;
            }

            public void Clear()
            {
                this._internalDictionary.Clear();
            }
        }
    }
}