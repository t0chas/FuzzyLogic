using System;

namespace Tochas.FuzzyLogic
{
    public class FuzzySet<T> where T : struct, IConvertible
    {
        private T[] enumValues;
        private FuzzyVariable<T>[] _variables;

        public FuzzySet()
        {
            FuzzyUtils.IsGenericParameterValid<T>();
            this.enumValues = FuzzyUtils.GetEnumValues<T>();
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
            int idx = Array.IndexOf<T>(this.enumValues, fuzzyVariable.LinguisticVariable);
            this._variables[idx] = fuzzyVariable;
        }

        public FuzzyVariable<T> Get(T linguisticVar)
        {
            int idx = Array.IndexOf<T>(this.enumValues, linguisticVar);
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
    }
}
