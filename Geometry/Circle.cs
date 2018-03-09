namespace Geometry
{
    public struct Circle
    {
        public readonly Vector2 Center;
        public readonly double R;

        public Circle(Vector2 center, double r)
        {
            Center = center;
            R = r;
        }

        public Circle(double x, double y, double r) : this(new Vector2(x, y), r)
        {

        }
    }
}