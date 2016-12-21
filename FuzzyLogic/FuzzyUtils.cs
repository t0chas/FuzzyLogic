using System;

namespace Tochas.FuzzyLogic
{
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
}
