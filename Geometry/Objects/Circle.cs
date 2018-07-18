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

        public bool Equals(Circle other)
        {
            return Center.Equals(other.Center) && R.Equal(other.R);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Circle && Equals((Circle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Center.GetHashCode() * 397) ^ R.GetHashCode();
            }
        }
    }
}