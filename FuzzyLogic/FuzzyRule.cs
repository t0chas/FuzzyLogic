using System;
using System.Collections.Generic;
using System.Linq;
using Tochas.FuzzyLogic.MembershipFunctions;

namespace Tochas.FuzzyLogic
{
    public interface IMemebershipFunction
    {
        float RepresentativeValue { get; }
        float fX(float x);
    }

    

    public static class Mathx
    {
        public static float Lerp(float y1, float y2, float x1, float x2, float x)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return y1 + ((x - x1) * dy / dx);
        }

        public static float Clamp(float min, float max, float val)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        public static float Clamp01(float val)
        {
            return Clamp(0f, 1f, val);
        }
    }

    public class FuzzyVariable<T> where T : struct, IConvertible
    {
        public T LinguisticVariable { get; private set; }
        public IMemebershipFunction MembershipFunction { get; private set; }

        public FuzzyVariable(T linguisticVariable, IMemebershipFunction membershipFunction)
        {
            this.LinguisticVariable = linguisticVariable;
            this.MembershipFunction = membershipFunction;
        }

        public FuzzyValue<T> fX(float x)
        {
            float y = this.MembershipFunction.fX(x);
            return new FuzzyValue<T>(this.LinguisticVariable, y);
        }

        public IFuzzyExpression AsExpression()
        {
            return new FuzzyVariableExpression<T>(this.LinguisticVariable);
        }

        public IFuzzyExpression Expr()
        {
            return this.AsExpression();
        }

        public static implicit operator FuzzyVariableExpression<T>(FuzzyVariable<T> self)
        {
            return new FuzzyVariableExpression<T>(self.LinguisticVariable);
        }
    }

    public class FuzzySet<T> where T : struct, IConvertible
    {
        private T[] enumValues;
        private FuzzyVariable<T>[] _variables;

        public FuzzySet()
        {
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
            for(int i=0; i< this._variables.Length; i++)
            {
                if (this._variables[i] == null)
                    continue;
                FuzzyValue<T> value = this._variables[i].fX(x);
                outputs.SetValue(value);
            }
        }
    }

    public struct FuzzyValue<T> where T : struct, IConvertible
    {
        public T linguisticVariable;
        public float membershipDegree;

        public float Confidence
        {
            get { return this.membershipDegree; }
            set { this.membershipDegree = value; }
        }

        public FuzzyValue(T lingVar, float degree)
        {
            this.linguisticVariable = lingVar;
            this.membershipDegree = degree;
        }
    }

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

    public interface IFuzzyOperator : IFuzzyExpression
    {
        //float Evaluate(FuzzySet set);
    }

    public interface IFuzzyExpression
    {
        float Evaluate(FuzzyValueSet set);
    }

    public class FuzzyVariableExpression<T> : IFuzzyExpression where T : struct, IConvertible
    {
        public T LinguisticVariable { get; private set; }

        public FuzzyVariableExpression(T linguisticVar)
        {
            this.LinguisticVariable = linguisticVar;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var fuzzyVar = set.GetValue(this.LinguisticVariable);
            return Mathx.Clamp01(fuzzyVar.membershipDegree);
        }

        public override string ToString()
        {
            return this.LinguisticVariable.ToString();
        }
    }

    public class FuzzyAnd : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression OperandA { get; private set; }
        public IFuzzyExpression OperandB { get; private set; }

        public FuzzyAnd(IFuzzyExpression operandA, IFuzzyExpression operandB)
        {
            this.OperandA = operandA;
            this.OperandB = operandB;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var valA = this.OperandA.Evaluate(set);
            var valB = this.OperandB.Evaluate(set);
            return Mathx.Clamp01(Math.Min(valA, valB));
        }

        public override string ToString()
        {
            return string.Format("({0} AND {1})", this.OperandA, this.OperandB);
        }
    }

    public class FuzzyOr : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression OperandA { get; private set; }
        public IFuzzyExpression OperandB { get; private set; }

        public FuzzyOr(IFuzzyExpression operandA, IFuzzyExpression operandB)
        {
            this.OperandA = operandA;
            this.OperandB = operandB;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var valA = this.OperandA.Evaluate(set);
            var valB = this.OperandB.Evaluate(set);
            return Mathx.Clamp01(Math.Max(valA, valB));
        }

        public override string ToString()
        {
            return string.Format("({0} OR {1})", this.OperandA, this.OperandB);
        }
    }

    public class FuzzyNot : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyNot(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = Mathx.Clamp01(this.Expression.Evaluate(set));
            float ret = 1.0f - val;
            return ret;
        }

        public override string ToString()
        {
            return string.Format("NOT({0})", this.Expression);
        }

    }

    public class FuzzyVery : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyVery(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = Mathx.Clamp01(this.Expression.Evaluate(set));
            val = val * val;
            return val;
        }

        public override string ToString()
        {
            return string.Format("VERY({0})", this.Expression);
        }

    }

    public class FuzzyFairly : IFuzzyOperator, IFuzzyExpression
    {
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyFairly(IFuzzyExpression exp)
        {
            this.Expression = exp;
        }

        public float Evaluate(FuzzyValueSet set)
        {
            var val = Mathx.Clamp01(this.Expression.Evaluate(set));
            val = (float)Math.Sqrt(val);
            return val;
        }

        public override string ToString()
        {
            return string.Format("FAIRLY({0})", this.Expression);
        }

    }

    public class FuzzyRule<T> where T : struct, IConvertible
    {
        public T OutputLinguisticVariable { get; private set; }
        public IFuzzyExpression Expression { get; private set; }

        public FuzzyRule(T outputVar, IFuzzyExpression exp)
        {
            this.OutputLinguisticVariable = outputVar;
            this.Expression = exp;
        }

        public FuzzyValue<T> Evaluate(FuzzyValueSet set)
        {
            FuzzyValue<T> ret = new FuzzyValue<T>();
            ret.linguisticVariable = this.OutputLinguisticVariable;
            ret.membershipDegree = Mathx.Clamp01(this.Expression.Evaluate(set));
            return ret;
        }

        public override string ToString()
        {
            return string.Format("IF {0} THEN {1}", this.Expression, this.OutputLinguisticVariable);
        }
    }

    public static class FuzzyUtils
    {
        public static T[] GetEnumValues<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var enumValuesArr = Enum.GetValues(typeof(T));
            var enumValues = new T[enumValuesArr.Length];
            for (int i = 0; i < enumValuesArr.Length; i++)
            {
                enumValues[i] = (T)enumValuesArr.GetValue(i);
            }
            return enumValues;
        }
    }

    public class FuzzyRuleSet<T> where T: struct, IConvertible
    {
        private List<FuzzyRule<T>> rules;
        public FuzzySet<T> OutputVarSet { get; private set; }

        public FuzzyValueSet OutputValueSet { get; private set; }

        public IRuleEvaluator<T> RuleEvaluator { get; set; }
        public IFuzzyValuesMerger<T> OutputsMerger { get; set; }
        public IDefuzzer<T> Defuzzer { get; set; }

        public FuzzyRuleSet(FuzzySet<T> varSet, List<FuzzyRule<T>> rules, IRuleEvaluator<T> ruleEvaluator, IFuzzyValuesMerger<T> outputsMerger, IDefuzzer<T> defuzzer)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
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

    public static class OperatorsExtensions
    {
        public static IFuzzyExpression Expr<T>(this T enumValue) where T:struct, IConvertible
        {
            return new FuzzyVariableExpression<T>(enumValue);
        }

        public static IFuzzyExpression And(this IFuzzyExpression exp, IFuzzyExpression other)
        {
            FuzzyAnd and = new FuzzyAnd(exp, other);
            return and;
        }

        public static IFuzzyExpression And<T>(this IFuzzyExpression exp, FuzzyVariable<T> fuzzVar) where T : struct, IConvertible
        {
            return exp.And(fuzzVar.AsExpression());
        }

        public static IFuzzyExpression Or(this IFuzzyExpression exp, IFuzzyExpression other)
        {
            FuzzyOr or = new FuzzyOr(exp, other);
            return or;
        }

        public static IFuzzyExpression Not(this IFuzzyExpression exp)
        {
            FuzzyNot not = new FuzzyNot(exp);
            return not;
        }

        public static FuzzyRule<T> Then<T>(this IFuzzyExpression exp, T outputLinguisticVar) where T : struct, IConvertible
        {
            FuzzyRule<T> rule = new FuzzyRule<T>(outputLinguisticVar, exp);
            return rule;
        }
    }
}
