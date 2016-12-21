using System.Linq;

namespace Tochas.FuzzyLogic.MembershipFunctions
{
    public class ShoulderMembershipFunction : IMemebershipFunction
    {
        private Coords[] points;

        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public Coords P0 { get { return this.points[0]; } }
        public Coords P1 { get { return this.points[1]; } }

        public ShoulderMembershipFunction(float minX, Coords p0, Coords p1, float maxX)
        {
            this.MinX = minX;
            this.MaxX = maxX;
            this.SetPoints(p0, p1);
        }

        public void SetPoints(Coords p0, Coords p1)
        {
            if (this.points == null)
                this.points = new Coords[2];
            this.points[0] = p0;
            this.points[1] = p1;
            this.points = this.points.OrderBy(x => x.X).ToArray();
        }

        public float fX(float x)
        {
            if (x <= this.P0.X)
                return this.P0.Y;
            if (x >= this.P1.X)
                return this.P1.Y;
            return Coords.Lerp(this.P0, this.P1, x);
        }

        public float RepresentativeValue
        {
            get
            {
                if (this.P0.Y > this.P1.Y)
                    return (this.P0.X + this.MinX) * 0.5f;
                return (this.MaxX + this.P1.X) * 0.5f;
            }
        }
    }
}
