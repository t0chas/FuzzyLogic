using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    public static class FuzzyUtils
    {
        private static Dictionary<System.Type, bool> _cachedParameters;

        static FuzzyUtils()
        {
            _cachedParameters = new Dictionary<System.Type, bool>(100);
        }

        public static bool IsGenericParameterValid<T>() where T : struct, System.IConvertible
        {
            /*System.Type typeT = typeof(T);
            bool result = false;
            if (_cachedParameters.TryGetValue(typeT, out result))
            {
                return result;
            }
            else
            {
                result = typeT.IsEnum && System.Type.GetTypeCode(typeT) == System.TypeCode.Int32;
                _cachedParameters[typeT] = result;
                if (!result)
                {
                    throw new System.ArgumentException("T must be an int-enumerated type");
                }
                return result;
            }*/
            return true;
        }

        public static T[] GetEnumValues<T>() where T : struct, System.IConvertible
        {
            IsGenericParameterValid<T>();
            var enumValuesArr = System.Enum.GetValues(typeof(T));
            var enumValues = new T[enumValuesArr.Length];
            for (int i = 0; i < enumValuesArr.Length; i++)
            {
                enumValues[i] = (T)enumValuesArr.GetValue(i);
            }
            return enumValues;
        }
    }
}
