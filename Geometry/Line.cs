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
            A = y1 - y2;
            B = x2 - x1;
            C = x1 * y2 - x2 * y1;
        }

        public Line(Vector2 vector, double x, double y)
        {
            A = vector.Y;
            B = -vector.X;
            C = vector.X * y - vector.Y * x;
        }

        public Line(Ray ray) : this(ray.Vector2, ray.Begin.X, ray.Begin.Y)
        {

        }

        public double? GetValue(double x)
        {
            if (B.Equal(0))
                return null;

            return (-C - A * x) / B;
        }
    }
}