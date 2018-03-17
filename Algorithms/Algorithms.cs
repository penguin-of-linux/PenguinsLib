using System.Collections.Generic;
using System.Linq;

using Geometry;

namespace Algorithms
{
    public static class AlgorithmsMethods
    {
        public static Vector2[] FindPathAmongRectangles(Vector2 start, Vector2 finish, params SimplifiedRectangle[] rectangles)
        {
            var searchRect = new SimplifiedRectangle(start, finish);
            rectangles = rectangles
                .Where(e => GeometryMethods.IsSimplifiedRectanglesInterescting(searchRect, e))
                .ToArray();

            var queue = new Queue<Vector2>();
            queue.Enqueue(start);
            var visited = new HashSet<Vector2> {start};
            var parent = new Dictionary<Vector2, Vector2?>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (!GeometryMethods.IsSegmentIntersectingRectangles(new Segment(current, finish), rectangles, false))
                {
                    parent[finish] = current;
                    break;
                }

                foreach (var n in GetNeighbors(current, rectangles, visited))
                {
                    parent[n] = current;
                    visited.Add(n);
                    queue.Enqueue(n);
                }
            }

            var result = new LinkedList<Vector2>();
            var a = finish;
            while (parent.ContainsKey(a))
            {
                result.AddFirst(a);
                a = parent[a].Value;
            }

            return result.ToArray();
        }

        private static IEnumerable<Vector2> GetNeighbors(Vector2 point, SimplifiedRectangle[] rectangles, HashSet<Vector2> visited)
        {
            var result = new List<Vector2>();

            foreach (var rect in rectangles)
            {
                var hasPoint = rect.HasPoint(point);

                foreach (var v in rect.Vertexes)
                {
                    if (visited.Contains(v)) // для скорости запихал сюда
                        continue;

                    if (hasPoint)
                    {
                        if (v.X.Equal(point.X) && !v.Y.Equal(point.Y) || !v.X.Equal(point.X) && v.Y.Equal(point.Y))
                            result.Add(v);
                    }
                    else
                    {
                        if (!GeometryMethods.IsSegmentIntersectingRectangles(new Segment(point, v), rectangles, false))
                            result.Add(v);
                    }
                }
            }

            return result;
        }
    }
}
