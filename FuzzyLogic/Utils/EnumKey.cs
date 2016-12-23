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
}
