using System;

namespace Geometry
{
    public struct Vector2
    {
        public double Length => Math.Sqrt(X * X + Y * Y);

        public readonly double X;
        public readonly double Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return v1 + (-v2);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator *(Vector2 v, double k)
        {
            return new Vector2(v.X * k, v.Y * k);
        }

        public static Vector2 operator *(double k, Vector2 v)
        {
            return v * k;
        }

        public static implicit operator Vector2(IntVector2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public Vector2 GetNormalized()
        {
            if (Length.Equal(0))
                return new Vector2(0, 0);

            var x = X / Length;
            var y = Y / Length;

            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }

        public override bool Equals(object other)
        {
            if (other is Vector2 vector2)
                return Equals(vector2);

            if (other is IntVector2 intVector2)
                return Equals(intVector2);

            return false;
        }

        private bool Equals(Vector2 other)
        {
            return X.Equal(other.X) && Y.Equal(other.Y);
        }

        private bool Equals(IntVector2 other)
        {
            Vector2 vec = other;
            return Equals(vec);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X.GetHashCode() * 397 + Y.GetHashCode() * 7;
            }
        }
    }
}