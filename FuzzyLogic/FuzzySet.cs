using System;
using Tochas.FuzzyLogic.Utils;

namespace Tochas.FuzzyLogic
{
    public class FuzzySet<T> where T : struct, IConvertible
    {
        private EnumKey<T>[] enumValues;
        private FuzzyVariable<T>[] _variables;

        public FuzzySet()
        {
            FuzzyUtils.IsGenericParameterValid<T>();
            var enumArr = FuzzyUtils.GetEnumValues<T>();
            this.enumValues = new EnumKey<T>[enumArr.Length];
            for(int i=0; i< this.enumValues.Length; i++)
            {
                this.enumValues[i] = EnumKey<T>.From(enumArr[i]);
            }
            this._variables = new FuzzyVariable<T>[this.enumValues.Length];
        }

        public void Set(T linguisticVariable, IMemebershipFunction fx)
        {
            this.Set(new FuzzyVariable<T>(linguisticVariable, fx));
        }

        public void Set(FuzzyVariable<T> fuzzyVariable)
        {
            if (fuzzyVariable == null)
                return;
            int idx = Array.IndexOf<EnumKey<T>>(this.enumValues, EnumKey<T>.From(fuzzyVariable.LinguisticVariable));
            this._variables[idx] = fuzzyVariable;
        }

        public FuzzyVariable<T> Get(T linguisticVar)
        {
            int idx = Array.IndexOf<EnumKey<T>>(this.enumValues, EnumKey<T>.From(linguisticVar));
            return this._variables[idx];
        }

        public IFuzzyExpression Expr(T linguisticVar)
        {
            var fuzzVar = this.Get(linguisticVar);
            return fuzzVar.Expr();
        }

        public bool IsValid()
        {
            for (int i = 0; i < this._variables.Length; i++)
                if (this._variables[i] == null)
                    return false;
            return true;
        }

        public void Evaluate(float x, FuzzyValueSet outputs)
        {
            if (outputs == null)
                return;
            for (int i = 0; i < this._variables.Length; i++)
            {
                if (this._variables[i] == null)
                    continue;
                FuzzyValue<T> value = this._variables[i].fX(x);
                outputs.Set(value);
            }
        }

        public T Evaluate(float x)
        {
            FuzzyValueSet valueSet = new FuzzyValueSet();
            this.Evaluate(x, valueSet);

            T maxKey = default(T);
            float maxDegree = float.MinValue;

            foreach(EnumKey<T> key in this.enumValues)
            {
                FuzzyValue<T> var = valueSet.Get(key.Value);
                if(var.Confidence > maxDegree)
                {
                    maxKey = var.linguisticVariable;
                    maxDegree = var.Confidence;
                }
            }
            return maxKey;
        }
    }
}
