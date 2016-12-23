using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    public partial class FuzzyValueSet
    {
        private class Container
        {
            /// <summary>
            /// http://www.somasim.com/blog/2015/08/c-performance-tips-for-unity-part-2-structs-and-enums/
            /// http://stackoverflow.com/questions/26280788/dictionary-enum-key-performance
            /// </summary>
            private struct EnumKey : System.IEquatable<EnumKey>
            {
                private int typeHash;
                private int valueHash;

                public static EnumKey From<T>(T value) where T : struct, System.IConvertible
                {
                    FuzzyUtils.IsGenericParameterValid<T>();
                    EnumKey key = new EnumKey();
                    key.typeHash = typeof(T).GetHashCode();
                    key.valueHash = EnumInt32ToInt.Convert(value);
                    return key;
                }

                public bool Equals(EnumKey other)
                {
                    return other.typeHash == this.typeHash && other.valueHash == this.valueHash;
                }

                public override bool Equals(object obj)
                {
                    System.Type otherType = obj.GetType();
                    if (!otherType.IsValueType && obj == null)
                        return false;
                    if (otherType != typeof(EnumKey))
                        return false;
                    EnumKey other = (EnumKey)obj;
                    return other.typeHash == this.typeHash && other.valueHash == this.valueHash;
                }

                public override int GetHashCode()
                {
                    return this.typeHash * 17 + this.valueHash;
                }
            }

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