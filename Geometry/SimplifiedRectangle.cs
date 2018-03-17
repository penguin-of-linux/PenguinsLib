using System;
using System.Collections.Generic;

namespace Geometry
{
    public struct SimplifiedRectangle
    {
        public readonly Vector2 V1;
        public readonly Vector2 V2;

        public readonly double Height;
        public readonly double Width;

        /// <summary>
        /// Rectangle with parallel to axis OY OX sides
        /// </summary>
        public SimplifiedRectangle(Vector2 v1, Vector2 v2) : this(v1.X, v1.Y, v2.X, v2.Y)
        {
        }

        /// <summary>
        /// Rectangle with parallel to axis OY OX sides
        /// </summary>
        public SimplifiedRectangle(double x1, double y1, double x2, double y2)
        {
            V1 = new Vector2(x1, y1);
            V2 = new Vector2(x2, y2);

            Width = Math.Abs(x1 - x2);
            Height = Math.Abs(y1 - y2);
        }

        public bool HasPoint(Vector2 point, bool withInsides = false)
        {
            if (withInsides) throw new NotImplementedException("I am so lazy");

            if (point.Equals(V1)) return true;
            if (point.Equals(V2)) return true;
            if (point.Equals(new Vector2(V1.X, V2.Y))) return true;
            if (point.Equals(new Vector2(V2.X, V1.Y))) return true;

            return false;
        }

        public IEnumerable<Vector2> Vertexes => new[]
        {
            V1, V2, new Vector2(V1.X, V2.Y), new Vector2(V2.X, V1.Y)
        };

        public bool HasPoint(double x, double y, bool withInsides = false)
        {
            return HasPoint(new Vector2(x, y), withInsides);
        }

        public Segment[] Segments => new[]
        {
            new Segment(V1.X, V1.Y, V2.X, V1.Y),
            new Segment(V1.X, V1.Y, V1.X, V2.Y),
            new Segment(V2.X, V1.Y, V2.X, V2.Y),
            new Segment(V1.X, V2.Y, V2.X, V2.Y)
        };
    }
}