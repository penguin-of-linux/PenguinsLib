using System;

namespace Geometry
{
    public struct Vector3
    {
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return v1 + (-v2);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator *(Vector3 v, double k)
        {
            return new Vector3(v.X * k, v.Y * k, v.Z * k);
        }

        public static Vector3 operator *(double k, Vector3 v)
        {
            return v * k;
        }

        public Vector3 GetNormalized()
        {
            if (Length.Equal(0))
                return new Vector3(0, 0, 0);

            var x = X / Length;
            var y = Y / Length;
            var z = Z / Length;

            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        public override bool Equals(object other)
        {
            if (other is Vector3 vector3)
                return Equals(vector3);

            return false;
        }

        private bool Equals(Vector3 other)
        {
            return X.Equal(other.X) && Y.Equal(other.Y) && Z.Equal(other.Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X.GetHashCode() * 397 + Y.GetHashCode() * 7 + Z.GetHashCode() * 13;
            }
        }
    }
}