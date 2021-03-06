﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Geometry
{
    public static class GeometryMethods
    {
        /// <summary>
        /// Returns angle between vectors counterclock-wise
        /// </summary>
        public static double GetAngleBetweenVectorCww(Vector2 v1, Vector2 v2)
        {
            var p = v1.X * v2.X + v1.Y * v2.Y;
            var cos = p / (v1.Length * v2.Length);
            var angle = Math.Acos(cos);

            if (cos.Equal(1))
                return 0;

            if (cos.Equal(-1))
                return Math.PI;

            // векторное произведение векторов на плоскости в базисе [(1, 0), (0, 1)]
            var c = new Vector3(0, 0, 1) * (v1.X * v2.Y - v2.X * v1.Y);

            if (c.Z > 0)
                return angle;

            return 2 * Math.PI - angle;
        }

        /// <summary>
        /// Angle difference, conterclock-wise. Those how much to add to a1 to get a2.
        /// </summary>
        /// <returns></returns>
        public static double GetAnglesDifference(double a1, double a2)
        {
            a1 = GetNormalizedAngle(a1);
            a2 = GetNormalizedAngle(a2);

            if (a1 > a2)
            {
                var cw = a1 - a2; // clockwise
                var ccw = 2 * Math.PI - cw; // counter cw

                return cw < ccw ? -cw : ccw;
            }
            else
            {
                var ccw = a2 - a1;
                var cw = 2 * Math.PI - ccw;

                return cw < ccw ? -cw : ccw;
            }
        }

        /// <summary>
        /// Distance on plane.
        /// </summary>
        public static double GetDistance(Vector2 v1, Vector2 v2)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }

        /// <summary>
        /// Distance from point to segment.
        /// </summary>
        public static double GetDistanceToSegment(Vector2 point, Segment segment)
        {
            var npoint = GetNearestPointOfSegment(point, segment);
            return GetDistance(point, npoint);
        }

        /// <summary>
        /// Line and circle intersection. Returns Point, Points, None
        /// </summary>
        public static IntersectionResult GetLineCircleIntersection(Line line, Circle circle, out object result)
        {
            var pline = new Line(new Vector2(line.A, line.B), circle.Center.X, circle.Center.Y);
            GetLinesIntersection(line, pline, out var intres);
            var ipoint = (Vector2) intres;  // перпендикулярные прямые, всегда точка

            var dist = GetDistance(circle.Center, ipoint);

            if (dist.Greater(circle.R))
            {
                result = null;
                return IntersectionResult.None;
            }


            if (dist.Equal(circle.R, 0.01)) // боюсь за точность тут
            {
                result = ipoint;
                return IntersectionResult.Point;
            }

            var d = Math.Sqrt(circle.R * circle.R + dist * dist);
            var a = Math.Sqrt(d * d / (line.A * line.A + line.B * line.B));
            var v = new Vector2(a, a);

            result = new[] { ipoint + v, ipoint - v };
            return IntersectionResult.Points;
        }

        /// <summary>
        /// Lines intersection. Returns Line, Point, None.
        /// </summary>
        public static IntersectionResult GetLinesIntersection(Line line1, Line line2, out object result)
        {
            /*// если прямые вертикальны
            if (line1.B.Equal(0) && line2.B.Equal(0))
            {
                // A и B не могут быть одновременно нулями, см. конструкторы Line
                if ((-line1.C / line1.A).Equal(-line2.C / line2.A)) 
                {
                    // если x1 == x2, то прямые совпадают
                    result = line1;
                    return IntersectionResult.Line;
                }

                result = null;
                return IntersectionResult.None;
            }

            if (line1.A.Equal(0) && line2.A.Equal(0))*/

            var v1 = new Vector2(-line1.B, line1.A);
            var v2 = new Vector2(-line2.B, line2.A);

            if (IsVectorsCollinear(v1, v2))
            {
                if (v1.X.Equal(0)) // если прямые вертикальны
                {
                    // A и B не могут быть одновременно нулями, см. конструкторы Line
                    if ((-line1.C / line1.A).Equal(-line2.C / line2.A))
                    {
                        // если x1 == x2, то прямые совпадают
                        result = line1;
                        return IntersectionResult.Line;
                    }

                    result = null;
                    return IntersectionResult.None;
                }
                else
                {
                    if (line1.C.Equal(line2.C))
                    {
                        result = line1;
                        return IntersectionResult.Line;
                    }
                    else
                    {
                        result = null;
                        return IntersectionResult.None;
                    }
                }
            }
            
            // вектора не коллинеарны
            try
            {
                var x = (line1.C * line2.B - line2.C * line1.B) / (line2.A * line1.B - line1.A * line2.B);
                var nonYLine = line1.B != 0 ? line1 : line2; // ищем прямую с B != 0
                var y = -(nonYLine.C + nonYLine.A * x) / nonYLine.B;

                if (double.IsNaN(x) || double.IsNaN(y))
                {
                    // прямые совпадают
                    result = line1;
                    return IntersectionResult.Line;
                }

                result = new Vector2(x, y);
                return IntersectionResult.Point;
            }
            catch (DivideByZeroException)
            {
                result = null;
                return IntersectionResult.None;
            }
        }

        /// <summary>
        /// Returns value of function that define by line. If line is vertical then returns null;
        /// </summary>
        public static double? GetLineFunctionValue(Line line, double arg)
        {
            if (line.B.Equal(0))
                return null;

            return (-line.C - line.A * arg) / line.B;
        }


        /// <summary>
        /// Line and segment intersection. Returns Segment, Point, None.
        /// </summary>
        public static IntersectionResult GetLineSegmentIntersection(Line line, Segment segment, out object result)
        {
            var segmentLine = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var intersection = GetLinesIntersection(line, segmentLine, out var intres);

            if (intersection == IntersectionResult.Line)
            {
                result = segment;
                return IntersectionResult.Segment;
            }

            var iPoint = (Vector2) intres; // intersection point

            if (HasSegmentPoint(segment, iPoint))
            {
                result = iPoint;
                return IntersectionResult.Point;
            }
            else
            {
                result = null;
                return IntersectionResult.None;
            }
        }

        /// <summary>
        /// Nearest point of segment to given point.
        /// </summary>
        public static Vector2 GetNearestPointOfSegment(Vector2 point, Segment segment)
        {
            var segmentLine = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var pVector = new Vector2(segmentLine.A, segmentLine.B); // perpendicular vector
            var pLine = new Line(pVector, point.X, point.Y); // perpendicular point

            var intersection = GetLineSegmentIntersection(pLine, segment, out var intres); // только Point

            if (intersection == IntersectionResult.Point)
                return (Vector2) intres;

            var distToBegin = GetDistance(point, segment.Begin);
            var distToEnd = GetDistance(point, segment.End);

            return distToBegin < distToEnd ? segment.Begin : segment.End;
        }

        /// <summary>
        /// Returns angle int range [0, 2P) that equals given
        /// </summary>
        public static double GetNormalizedAngle(double a)
        {
            return a - Math.Floor(a / (2 * Math.PI)) * 2 * Math.PI;
        }

        /// <summary>
        /// Ray and circle intersection. Returns Point, Point, None
        /// </summary>
        public static IntersectionResult GetRayCircleIntersection(Ray ray, Circle circle, out object result)
        {
            var line = new Line(ray);
            var intersection = GetLineCircleIntersection(line, circle, out var intres);

            if (intersection == IntersectionResult.None)
            {
                result = intres;
                return intersection;
            }

            if (intersection == IntersectionResult.Point)
            {
                var p = (Vector2) intres;
                if (HasRayPoint(ray, p))
                {
                    result = intres;
                    return IntersectionResult.Points;
                }
                else
                {
                    result = null;
                    return IntersectionResult.None;
                }
            }

            // intersection = Points
            var points = (Vector2[]) intres;
            var firstInRay = HasRayPoint(ray, points[0]);
            var secondInRay = HasRayPoint(ray, points[1]);

            if (firstInRay && secondInRay)
            {
                result = intres;
                return IntersectionResult.Points;
            }
            else if (firstInRay)
            {
                result = points[0];
                return IntersectionResult.Point;
            }
            else if (secondInRay)
            {
                result = points[1];
                return IntersectionResult.Point;
            }
            else
            {
                result = null;
                return IntersectionResult.None;
            }
        }

        /// <summary>
        /// Ray and line intersection. Return Ray, Point, None
        /// </summary>
        public static IntersectionResult GetRayLineIntersection(Ray ray, Line line, out object result)
        {
            var rayLine = new Line(ray.Vector, ray.Begin.X, ray.Begin.Y);
            var intersection = GetLinesIntersection(line, rayLine, out var intres);

            switch (intersection)
            {
                case IntersectionResult.Line:
                    result = ray;
                    return IntersectionResult.Ray;
                case IntersectionResult.None:
                    result = null;
                    return IntersectionResult.None;
                case IntersectionResult.Point:
                    if (HasRayPoint(ray, (Vector2)intres))
                    {
                        result = intres;
                        return IntersectionResult.Point;
                    }
                    else
                    {
                        result = null;
                        return IntersectionResult.None;
                    }
            }

            throw new Exception($"Internal error. GetLinesIntersection returned {intersection}");
        }

        /// <summary>
        /// Ray and segment intersection. Returns Segment, Point None.
        /// </summary>
        public static IntersectionResult GetRaySegmentIntersection(Ray ray, Segment segment, out object result)
        {
            var sline = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var intersection = GetRayLineIntersection(ray, sline, out var intres);

            switch (intersection)
            {
                case IntersectionResult.Ray:
                    result = ray;
                    return IntersectionResult.Ray;
                case IntersectionResult.None:
                    result = null;
                    return IntersectionResult.None;
                case IntersectionResult.Point:
                    if (HasRayPoint(ray, (Vector2)intres))
                    {
                        result = intres;
                        return IntersectionResult.Point;
                    }
                    else
                    {
                        result = null;
                        return IntersectionResult.None;
                    }
            }

            throw new Exception($"Internal error. GetRaySegmentIntersection returned {intersection}");
        }

        /// <summary>
        /// Segments intersection. Returns Segment, Point, None
        /// </summary>
        public static IntersectionResult GetSegmentsIntersection(Segment seg1, Segment seg2, out object result)
        {
            var intersection = GetLinesIntersection(new Line(seg1), new Line(seg2), out var intres);

            if (intersection == IntersectionResult.None)
            {
                result = null;
                return IntersectionResult.None;
            }

            if (intersection == IntersectionResult.Point)
            {
                var iPoint = (Vector2) intres;
                if (HasSegmentPoint(seg1, iPoint) && HasSegmentPoint(seg2, iPoint))
                {
                    /*if (!includeBorders)
                        if (seg1.Begin.Equals(iPoint) || seg2.Begin.Equals(iPoint)
                                                      || seg1.End.Equals(iPoint) || seg2.End.Equals(iPoint))
                            return null;*/

                    result = intres;
                    return IntersectionResult.Point;
                }

                result = null;
                return IntersectionResult.None;
            }

            // отрезки лежат на одной прямой
            return GetSegmentsOnLineIntersection(seg1, seg2, out result);
        }

        public static IntersectionResult GetSegmentSimplifiedRectangleIntersection(Segment seg, 
            SimplifiedRectangle rect, out object result, bool filledRectangle = false)
        {
            var points = new List<Vector2>();

            foreach (var s in rect.Segments)
            {
                var intersection = GetSegmentsIntersection(seg, s, out var intres);

                switch (intersection)
                {
                    case IntersectionResult.Segment:
                        result = intres;
                        return IntersectionResult.Segment;
                    case IntersectionResult.Point:
                        points.Add((Vector2)intres);
                        break;
                    case IntersectionResult.None:
                        continue;
                }
            }

            points = points.Distinct().ToList(); // ох ужас

            // если точек пересечения нет, то вернем None
            // если точка пересечения одна, то она либо вершина прямоугольника и тогда мы вернем ее,
            //     либо точка пересечения является вершиной отрезка, и тогда мы вернем ее, если not filledRectangle, 
            //     иначе вернем отрезок,
            // если точек пересечения две, то вернем либо отрезок, либо 2 точки в зависимости от filledRectangle

            var beginInRect = IsPointInsideSimplifiedRectangle(seg.Begin, rect);
            var endInRect = IsPointInsideSimplifiedRectangle(seg.End, rect);

            if (points.Count == 0)
            {
                if (beginInRect && endInRect && filledRectangle)
                {
                    result = seg;
                    return IntersectionResult.Segment;
                }
                else
                {
                    result = null;
                    return IntersectionResult.None;
                }
            }

            if (points.Count == 1)
            {
                var point = points[0];

                if (filledRectangle)
                {
                    if (!beginInRect && !endInRect)
                    {
                        result = point;
                        return IntersectionResult.Point;
                    }

                    if (beginInRect && endInRect)
                    {
                        result = seg;
                        return IntersectionResult.Segment;
                    }

                    var pointInRect = beginInRect ? seg.Begin : seg.End;

                    if (pointInRect.Equals(point))
                    {
                        result = point;
                        return IntersectionResult.Point;
                    }
                    else
                    {
                        result = new Segment(pointInRect, point);
                        return IntersectionResult.Segment;
                    }
                }
                else
                {
                    result = points[0];
                    return IntersectionResult.Point;
                }
            }

            // точки две
            if (filledRectangle)
            {
                result = new Segment(points[0], points[1]);
                return IntersectionResult.Segment;
            }
            else
            {
                result = new[] { points[0], points[1] };
                return IntersectionResult.Points;
            }
        }

        /// <summary>
        /// Intersection of segments that lie on one line. Returns Segment, Point, None
        /// </summary>
        public static IntersectionResult GetSegmentsOnLineIntersection(Segment seg1, Segment seg2, out object result)
        {
            var axisChanged = false;

            if (seg1.Begin.X.Equal(seg1.End.X)) // если отрезки вертикальны
            {
                seg1 = new Segment(seg1.Begin.Y, seg1.Begin.X, seg1.End.Y, seg1.End.X);
                seg2 = new Segment(seg2.Begin.Y, seg2.Begin.X, seg2.End.Y, seg2.End.X);
                axisChanged = true;
            }

            var minx2 = Math.Min(seg2.Begin.X, seg2.End.X);
            var maxx2 = Math.Max(seg2.Begin.X, seg2.End.X);
            var minx1 = Math.Min(seg1.Begin.X, seg1.End.X);
            var maxx1 = Math.Max(seg1.Begin.X, seg1.End.X);

            if (minx1.Greater(maxx2) || minx2.Greater(maxx1)) // не пересекаются
            {
                result = null;
                return IntersectionResult.None;
            }

            var x1 = Math.Max(minx1, minx2);
            var x2 = Math.Min(maxx1, maxx2);

            var line = new Line(seg1);

            // решарпер не знает, что line не вертикальная, т.к. мы отразили отрезки в начале функции. Глупый решарпер.
            var y1 = GetLineFunctionValue(line, x1).Value;
            var y2 = GetLineFunctionValue(line, x2).Value;
            var seg = axisChanged ? new Segment(y1, x1, y2, x2) : new Segment(x1, y1, x2, y2);

            if (x1.Equal(x2))
            {
                result = new Vector2(x1, y1);
                return IntersectionResult.Point;
            }
            else
            {
                result = seg;
                return IntersectionResult.Segment;
            }
        }

        /// <summary>
        /// Проверяет, лежит ли точка на луче. Точка point лежит на линии луча.
        /// </summary>
        public static bool HasRayPoint(Ray ray, Vector2 point)
        {
            if (point.Equals(ray.Begin))
                return true;

            if (!ray.Vector.X.Equal(0))
            {
                if (ray.Vector.X.Greater(0))
                {
                    if (point.X.GreaterOrEqual(ray.Begin.X)) return true;
                    else return false;
                }
                else
                {
                    if (point.X.LessOrEqual(ray.Begin.X)) return true;
                    else return false;
                }
            }

            else
            {
                if (ray.Vector.Y.Greater(0))
                {
                    if (point.Y.GreaterOrEqual(ray.Begin.Y)) return true;
                    else return false;
                }
                else
                {
                    if (point.Y.LessOrEqual(ray.Begin.Y)) return true;
                    else return false;
                }
            }
        }

        /// <summary>
        /// Проверяет, лежит ли точка в отрезке. Точка point - точка, лежащая на линии отрезка
        /// </summary>
        public static bool HasSegmentPoint(Segment segment, Vector2 point)
        {
            var rspoint = segment.Begin.X.Greater(segment.End.X) ? segment.Begin : segment.End; // right segment point
            var lspoint = rspoint.Equals(segment.Begin) ? segment.End : segment.Begin;              // left segment point
            var tspoint = segment.Begin.Y.Greater(segment.End.Y) ? segment.Begin : segment.End; // top segment point
            var bspoint = tspoint.Equals(segment.Begin) ? segment.End : segment.Begin;              // bottom segment point

            return point.X.GreaterOrEqual(lspoint.X) && point.X.LessOrEqual(rspoint.X) &&
                   point.Y.GreaterOrEqual(bspoint.Y) && point.Y.LessOrEqual(tspoint.Y);
        }

        public static bool IsPointInsideSimplifiedRectangle(Vector2 point, SimplifiedRectangle rect)
        {
            return IsSimplifiedRectanglesInterescting(rect, new SimplifiedRectangle(point, point)); // :P смекалочка
        }

        public static bool IsSimplifiedRectanglesInterescting(SimplifiedRectangle rect1, SimplifiedRectangle rect2)
        {
            var minx1 = Math.Min(rect1.V1.X, rect1.V2.X);
            var maxx1 = Math.Max(rect1.V1.X, rect1.V2.X);
            var miny1 = Math.Min(rect1.V1.Y, rect1.V2.Y);
            var maxy1 = Math.Max(rect1.V1.Y, rect1.V2.Y);
            var minx2 = Math.Min(rect2.V1.X, rect2.V2.X);
            var maxx2 = Math.Max(rect2.V1.X, rect2.V2.X);
            var miny2 = Math.Min(rect2.V1.Y, rect2.V2.Y);
            var maxy2 = Math.Max(rect2.V1.Y, rect2.V2.Y);

            return (minx1 >= minx2 && minx1 <= maxx2 || maxx1 >= minx2 && maxx1 <= maxx2 || minx2 >= minx1 && maxx2 <= maxx1) &&
                   (miny1 >= miny2 && miny1 <= maxy2 || maxy1 >= miny2 && maxy1 <= maxy2 || miny2 >= miny1 && maxy2 <= maxy1);
        }

        public static bool IsVectorsCollinear(Vector2 v1, Vector2 v2)
        {
            var len1 = v1.Length;
            var len2 = v2.Length;
            if (len1.Equal(0) || len2.Equal(0))
                return true;

            var p = v1.X * v2.X + v1.Y * v2.Y;
            var cos = p / len1 / len2;

            return Math.Abs(cos).Equal(1);
        }
    }
}