namespace Geometry
{public struct Ray
    {
        public readonly Vector2 Begin;
        public readonly Vector2 Vector;

        public Ray(Vector2 begin, Vector2 vector)
        {
            Begin = begin;
            Vector = vector;
        }

        public bool Equals(Ray other)
        {
            return Begin.Equals(other.Begin) && Vector.Equals(other.Vector);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Ray && Equals((Ray)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Begin.GetHashCode() * 397) ^ Vector.GetHashCode();
            }
        }
    }
}