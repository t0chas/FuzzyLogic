using System;
using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    public class FuzzyValueSet
    {
        private Dictionary<IConvertible, object> values;

        public FuzzyValueSet()
        {
            this.values = new Dictionary<IConvertible, object>();
        }

        public void SetValue<T>(FuzzyValue<T> fuzzyValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            this.values[fuzzyValue.linguisticVariable] = fuzzyValue;
            //this.values.Add(fuzzyValue.linguisticVariable, fuzzyValue);
        }

        public FuzzyValue<T> GetValue<T>(T linguisticVariable) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            object varValue;
            if (this.values.TryGetValue(linguisticVariable, out varValue))
            {
                FuzzyValue<T> ret = default(FuzzyValue<T>);
                ret = (FuzzyValue<T>)varValue;
                return ret;
            }
            else
            {
                return default(FuzzyValue<T>);
            }
        }
    }
}
