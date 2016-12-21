using System;
using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    #region Fuzzification
    public interface IMemebershipFunction
    {
        float RepresentativeValue { get; }
        float fX(float x);
    }

    public interface IFuzzyOperator : IFuzzyExpression
    {
    }

    public interface IFuzzyExpression
    {
        float Evaluate(FuzzyValueSet set);
    }
    #endregion

    #region Inference process
    public interface IRuleEvaluator<T> where T : struct, IConvertible
    {
        FuzzyValue<T>[] EvaluateRules(List<FuzzyRule<T>> rules, FuzzyValueSet inputValueSet);
    }

    public interface IFuzzyValuesMerger<T> where T : struct, IConvertible
    {
        void MergeValues(FuzzyValue<T>[] values, FuzzyValueSet mergedOutputs);
    }

    public interface IDefuzzer<T> where T : struct, IConvertible
    {
        float Defuzze(FuzzySet<T> outputVariableSet, FuzzyValueSet fuzzyValues);
    }
    #endregion
}
