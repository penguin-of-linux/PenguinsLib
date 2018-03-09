using System;

namespace Geometry
{
    public struct IntVector2
    {
        public double Length => Math.Sqrt(X * X + Y * Y);

        public readonly int X;
        public readonly int Y;

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2)
        {
            return v1 + (-v2);
        }

        public static IntVector2 operator -(IntVector2 v)
        {
            return new IntVector2(-v.X, -v.Y);
        }

        public static IntVector2 operator *(IntVector2 v, int k)
        {
            return new IntVector2(v.X * k, v.Y * k);
        }

        public static IntVector2 operator *(int k, IntVector2 v)
        {
            return v * k;
        }

        public static implicit operator IntVector2(Vector2 vec)
        {
            return new IntVector2((int)vec.X, (int)vec.Y);
        }

        public IntVector2 GetNormalized()
        {
            if (Length.Equal(0))
                return new IntVector2(0, 0);

            var x = (int)(X / Length);
            var y = (int)(Y / Length);

            return new IntVector2(x, y);
        }

        public override bool Equals(object other)
        {
            if (other is IntVector2 vector2)
                return Equals(vector2);

            if (other is Vector2 vec)
                return Equals(vec);

            return false;
        }

        private bool Equals(IntVector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        private bool Equals(Vector2 other)
        {
            IntVector2 vec = other;
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