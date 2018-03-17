using System;

namespace Geometry
{
    public struct Line
    {
        public readonly double A;
        public readonly double B;
        public readonly double C;

        public Line(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Line(double x1, double y1, double x2, double y2)
        {
            if (x1.Equal(x2) && y1.Equal(y2))
                throw new ArgumentException("Equivalent points");

            A = y1 - y2;
            B = x2 - x1;
            C = x1 * y2 - x2 * y1;
        }

        public Line(Vector2 v1, Vector2 v2) : this(v1.X, v1.Y, v2.X, v2.Y)
        {
        }

        public Line(Vector2 vector, double x, double y)
        {
            if (vector.X.Equal(0) && vector.Y.Equal(0))
                throw new ArgumentException($"Vector cannot be zero");

            A = vector.Y;
            B = -vector.X;
            C = vector.X * y - vector.Y * x;
        }

        public Line(Ray ray) : this(ray.Vector, ray.Begin.X, ray.Begin.Y)
        {
        }

        public Line(Segment seg) : this(seg.Begin, seg.End)
        {
        }

        public double? GetValue(double x)
        {
            if (B.Equal(0))
                return null;

            return (-C - A * x) / B;
        }

        public bool Equals(Line other)
        {
            return A.Equal(other.A) && B.Equal(other.B) && C.Equal(other.C);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Line && Equals((Line)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = A.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ C.GetHashCode();
                return hashCode;
            }
        }
    }
}