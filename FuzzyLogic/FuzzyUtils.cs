using System;

namespace Tochas.FuzzyLogic
{
    public static class FuzzyUtils
    {
        public static bool IsGenericParameterValid<T>() where T : struct, IConvertible
        {
            Type typeT = typeof(T);
            if (!typeT.IsEnum && Type.GetTypeCode(typeT) != TypeCode.Int32)
            {
                throw new ArgumentException("T must be an int-enumerated type");
            }
            return true;
        }

        public static T[] GetEnumValues<T>() where T : struct, IConvertible
        {
            IsGenericParameterValid<T>();
            var enumValuesArr = Enum.GetValues(typeof(T));
            var enumValues = new T[enumValuesArr.Length];
            for (int i = 0; i < enumValuesArr.Length; i++)
            {
                enumValues[i] = (T)enumValuesArr.GetValue(i);
            }
            return enumValues;
        }
    }
}
