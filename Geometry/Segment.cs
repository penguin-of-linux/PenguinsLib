namespace Geometry
{
    public struct Segment
    {
        public readonly Vector2 Begin;
        public readonly Vector2 End;

        public Segment(Vector2 begin, Vector2 end)
        {
            Begin = begin;
            End = end;
        }

        public Segment(double x1, double y1, double x2, double y2)
            : this(new Vector2(x1, y1), new Vector2(x2, y2))
        {

        }
    }
}