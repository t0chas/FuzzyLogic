using System.Linq;

namespace Tochas.FuzzyLogic.MembershipFunctions
{
    /// <summary>
    /// Triangular membership function. 
    /// 
    ///                       P1
    ///                       /\ 
    ///                      /  \
    ///                     /    \
    ///                    /      \
    ///                   /        \
    ///                  /          \
    /// ----------------             -------------------------
    ///                P0            P2
    /// </summary>
    public class TriangularMemebershipFunction : IMemebershipFunction
    {

        private Coords[] points;

        public Coords P0 { get { return this.points[0]; } }
        public Coords P1 { get { return this.points[1]; } }
        public Coords P2 { get { return this.points[2]; } }

        public TriangularMemebershipFunction(Coords p0, Coords p1, Coords p2)
        {
            this.SetPoints(p0, p1, p2);
        }

        public void SetPoints(Coords p0, Coords p1, Coords p2)
        {
            if (this.points == null)
                this.points = new Coords[3];
            this.points[0] = p0;
            this.points[1] = p1;
            this.points[2] = p2;
            this.points = this.points.OrderBy(x => x.X).ToArray();
        }

        public float fX(float x)
        {
            if (x <= this.P0.X)
                return this.P0.Y;
            if (x >= this.P2.X)
                return this.P2.Y;
            if (x == this.P1.X)
                return this.P1.Y;
            if (x < this.P1.X)
            {
                return Coords.Lerp(this.P0, this.P1, x);
            }
            else
            {
                return Coords.Lerp(this.P1, this.P2, x);
            }
        }

        public float RepresentativeValue { get { return this.P1.X; } }
    }
}
