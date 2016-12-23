namespace Tochas.FuzzyLogic
{
    public struct FuzzyValue<T> : System.IEquatable<FuzzyValue<T>> where T : struct, System.IConvertible
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
            FuzzyUtils.IsGenericParameterValid<T>();
            this.linguisticVariable = lingVar;
            this.membershipDegree = degree;
        }

        public bool Equals(FuzzyValue<T> other)
        {
            int thisInt = EnumInt32ToInt.Convert(this.linguisticVariable);
            int otherInt = EnumInt32ToInt.Convert(other.linguisticVariable);
            return thisInt == otherInt && this.membershipDegree == other.membershipDegree;
        }

        public override bool Equals(object obj)
        {
            System.Type otherType = obj.GetType();
            if (!otherType.IsValueType && obj == null)
                return false;
            if (otherType != typeof(FuzzyValue<T>))
                return false;
            FuzzyValue<T> other = (FuzzyValue<T>)obj;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int thisInt = EnumInt32ToInt.Convert(this.linguisticVariable);
            return thisInt * 17 + this.membershipDegree.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}@{1}", this.linguisticVariable, this.membershipDegree);
        }
    }
}