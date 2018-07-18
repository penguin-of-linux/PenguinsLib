namespace Geometry
{
    public struct Segment
    {
        public readonly Vector2 Begin;
        public readonly Vector2 End;

        public Vector2[] Points => new[] { Begin, End };

        public Segment(Vector2 begin, Vector2 end)
        {
            Begin = begin;
            End = end;
        }

        public Segment(double x1, double y1, double x2, double y2)
            : this(new Vector2(x1, y1), new Vector2(x2, y2))
        {

        }

        public override string ToString()
        {
            return $"{Begin}, {End}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Begin.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        public bool Equals(Segment other)
        {
            return Begin.Equals(other.Begin) && End.Equals(other.End);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment && Equals((Segment) obj);
        }
    }
}