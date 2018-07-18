using System.Collections.Generic;
using System.Linq;

using Geometry;

namespace Algorithms
{
    public static class AlgorithmsMethods
    {
        /// <summary>
        /// Юзаем бфс :)
        /// </summary>
        public static Vector2[] FindPathAmongRectangles(Vector2 start, Vector2 finish, params SimplifiedRectangle[] rectangles)
        {
            //var begInRect = GeometryMethods.IsPointInsideSimplifiedRectangle(start, rect);
            //var endInRect = GeometryMethods.IsPointInsideSimplifiedRectangle(finish, rect);
            //if (begInRect && !endInRect || !begInRect && endInRect)
            //    return new Vector2[0];
            if (rectangles.Length == 0)
                return new Vector2[] { finish };

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
                var seg = new Segment(current, finish);

                if (!rectangles.Any(r => IsSegmentIntersectingRectangle(seg, r)))
                {
                    parent[finish] = current;
                    break;
                }

                foreach (var n in GetNeighbors(current, rectangles, visited))
                {
                    parent[n] = current;
                    visited.Add(n);
                    queue.Enqueue(n);
                    // проверка на visited в GetNeighbors - для ускорения
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
            // очень медленно :C

            var result = new List<Vector2>();

            foreach (var rect in rectangles)
            {
                var hasPoint = rect.Vertexes.Contains(point);

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
                        var seg = new Segment(point, v);
                        if (rectangles.Any(r => IsSegmentIntersectingRectangle(seg, r)))
                            continue;

                        result.Add(v);
                    }
                }
            }

            return result;
        }

        private static bool IsSegmentIntersectingRectangle(Segment seg, SimplifiedRectangle rect)
        {
            var intersection =
                GeometryMethods.GetSegmentSimplifiedRectangleIntersection(seg, rect, out var intres, true);

            if (intersection == IntersectionResult.None)
            {
                return false;
            }

            if (intersection == IntersectionResult.Point)
            {
                if (intres.Equals(seg.Begin) || intres.Equals(seg.End))
                    return false;
            }

            if (intersection == IntersectionResult.Points)
            {
                var points = (Vector2[])intres;
                if (seg.Begin.Equals(points[0]) && seg.End.Equals(points[1]) ||
                    seg.Begin.Equals(points[1]) && seg.End.Equals(points[0]))
                    return false;
            }

            return true;
        }
    }
}
