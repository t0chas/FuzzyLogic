using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    public static class MathFz
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
}
