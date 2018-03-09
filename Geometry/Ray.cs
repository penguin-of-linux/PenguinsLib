namespace Geometry
{public struct Ray
    {
        public readonly Vector2 Begin;
        public readonly Vector2 Vector2;

        public Ray(Vector2 begin, Vector2 vector)
        {
            Begin = begin;
            Vector2 = vector;
        }
    }
}