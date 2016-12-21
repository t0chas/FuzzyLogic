using System;

namespace Tochas.FuzzyLogic
{
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
}
