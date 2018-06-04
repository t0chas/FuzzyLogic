using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tochas.FuzzyLogic.MembershipFunctions
{
    public class TrapezoidalMemebershipFunction : IMemebershipFunction
    {
        private Coords[] points;

        public Coords P0 { get { return this.points[0]; } }
        public Coords P1 { get { return this.points[1]; } }
        public Coords P2 { get { return this.points[2]; } }
        public Coords P3 { get { return this.points[3]; } }

        public TrapezoidalMemebershipFunction(Coords p0, Coords p1, Coords p2, Coords p3)
        {
            this.SetPoints(p0, p1, p2, p3);
        }

        public void SetPoints(Coords p0, Coords p1, Coords p2, Coords p3)
        {
            if (this.points == null)
                this.points = new Coords[4];
            this.points[0] = p0;
            this.points[1] = p1;
            this.points[2] = p2;
            this.points[3] = p3;
            this.points = this.points.OrderBy(x => x.X).ToArray();
            if (this.P1.Y != this.P2.Y)
                throw new ArgumentException("P1 and P2 must have equals y");
        }

        public float fX(float x)
        {
            if (x <= this.P0.X)
                return this.P0.Y;
            if (x >= this.P3.X)
                return this.P3.Y;
            if (x >= this.P1.X && x <= this.P2.X)
                return this.P1.Y;
            if (x < this.P1.X)
            {
                return Coords.Lerp(this.P0, this.P1, x);
            }
            else if(x > this.P2.X)
            {
                return Coords.Lerp(this.P2, this.P3, x);
            }
            return float.NaN;
        }

        public float RepresentativeValue
        {
            get
            {
                return this.P1.X + ((this.P2.X - this.P1.X) * 0.5f);
            }
        }
    }
}
