
namespace Tochas.FuzzyLogic.MembershipFunctions
{
    public struct Coords
    {
        public float X;
        public float Y;

        public Coords(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static float Lerp(Coords c1, Coords c2, float x)
        {
            return MathFz.Lerp(c1.Y, c2.Y, c1.X, c2.X, x);
        }
    }
}
