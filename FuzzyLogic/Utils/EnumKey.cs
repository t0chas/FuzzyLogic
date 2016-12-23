using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tochas.FuzzyLogic.Utils
{
    /// <summary>
    /// http://www.somasim.com/blog/2015/08/c-performance-tips-for-unity-part-2-structs-and-enums/
    /// http://stackoverflow.com/questions/26280788/dictionary-enum-key-performance
    /// </summary>
    public struct EnumKey : System.IEquatable<EnumKey>
    {
        private int typeHash;
        private int valueHash;

        internal int TypeHash { get { return this.typeHash; } }
        internal int ValueHash { get { return this.ValueHash; } }

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
            if (!otherType.IsValueType)
                return false;
            if (otherType == typeof(EnumKey))
            {
                EnumKey other = (EnumKey)obj;
                return this.Equals(other);
            }
            if (otherType == typeof(IEquatable<EnumKey>))
            {
                IEquatable<EnumKey> other = (IEquatable<EnumKey>)obj;
                return other.Equals(this);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.typeHash * 17 + this.valueHash;
        }
    }

    public struct EnumKey<T> : System.IEquatable<EnumKey<T>>, System.IEquatable<EnumKey> where T :struct, IConvertible
    {
        private int typeHash;
        private int valueHash;

        internal int TypeHash { get { return this.typeHash; } }
        internal int ValueHash { get { return this.ValueHash; } }

        private T enumValue;
        public T Value { get { return this.enumValue; } }

        public EnumKey(T value)
        {
            FuzzyUtils.IsGenericParameterValid<T>();
            this.typeHash = typeof(T).GetHashCode();
            this.valueHash = EnumInt32ToInt.Convert(value);
            this.enumValue = value;
        }

        public static EnumKey<T> From(T value)
        {
            EnumKey<T> key = new EnumKey<T>(value);
            return key;
        }

        public bool Equals(EnumKey<T> other)
        {
            return other.typeHash == this.typeHash && other.valueHash == this.valueHash;
        }

        public bool Equals(EnumKey other)
        {
            return other.TypeHash == this.typeHash && other.ValueHash == this.valueHash;
        }

        public override bool Equals(object obj)
        {
            System.Type otherType = obj.GetType();
            if (!otherType.IsValueType)
                return false;
            if (otherType == typeof(EnumKey))
            {
                EnumKey other = (EnumKey)obj;
                return this.Equals(other);
            }
            if (otherType == typeof(EnumKey<T>))
            {
                EnumKey<T> other = (EnumKey<T>)obj;
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.typeHash * 17 + this.valueHash;
        }
    }
}
