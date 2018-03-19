using System;
using System.Linq;

namespace Geometry
{
    public static class GeometryMethods
    {
        /// <summary>
        /// Ищет разницу между a1 и a2, против часовой стрелки
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
        /// Расстояние между двумя точками на плоскости.
        /// </summary>
        public static double GetDistance(Vector2 v1, Vector2 v2)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }

        /// <summary>
        /// Ищет расстояние от точки до отрезка
        /// </summary>
        public static double GetDistanceToSegment(Vector2 point, Segment segment)
        {
            var npoint = GetNearestPointOfSegment(point, segment);
            return GetDistance(point, npoint);
        }

        /// <summary>
        /// Пересечение прямой и окружности. Возвращает массис с 0-2 точками.
        /// </summary>
        public static Vector2[] GetLineCircleIntersection(Line line, Circle circle)
        {
            var pline = new Line(new Vector2(line.A, line.B), circle.Center.X, circle.Center.Y);
            var ipoint = GetLinesIntersection(line, pline).Value; // перпендикулярные прямые, всегда будет решение

            var dist = GetDistance(circle.Center, ipoint);

            if (dist.Greater(circle.R))
                return new Vector2[0];

            if (dist.Equal(circle.R, 0.01)) // боюсь за точность тут
            {
                return new[] { ipoint };
            }

            var d = Math.Sqrt(circle.R * circle.R + dist * dist);
            var a = Math.Sqrt(d * d / (line.A * line.A + line.B + line.B));
            var v = new Vector2(a, a);

            return new[] { ipoint + v, ipoint - v };
        }

        /// <summary>
        /// Ищет пересечение прямых. Возвращает (NaN, NaN), если прямые совпадают. Возвращает null, если точки нет
        /// </summary>
        public static Vector2? GetLinesIntersection(Line line1, Line line2)
        {
            //todo: обработка совпадения прямых
            try
            {
                var x = (line1.C * line2.B - line2.C * line1.B) / (line2.A * line1.B - line1.A * line2.B);
                var nonYLine = line1.B != 0 ? line1 : line2; // ищем прямую с B != 0
                var y = -(nonYLine.C + nonYLine.A * x) / nonYLine.B;

                return new Vector2(x, y);
            }
            catch (DivideByZeroException)
            {
                return null;
            }
        }


        /// <summary>
        /// Пересечение отрезка и прямой. НЕ обрабатывает ситуацию, когда таких точек несчетно много.
        /// </summary>
        public static Vector2? GetLineSegmentIntersection(Line line, Segment segment)
        {
            //todo: обработка ситуации с несчетным количеством точек

            var segmentLine = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var intersection = GetLinesIntersection(line, segmentLine);
            if (intersection == null)
                throw new Exception("Smth goes wrong");

            var iPoint = intersection.Value; // intersection point

            if (HasSegmentPoint(segment, iPoint))
            {
                return iPoint;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ищет ближайшую к данной точке точку отрезка
        /// </summary>
        public static Vector2 GetNearestPointOfSegment(Vector2 point, Segment segment)
        {
            var segmentLine = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var pVector = new Vector2(segmentLine.A, segmentLine.B); // perpendicular vector
            var pLine = new Line(pVector, point.X, point.Y); // perpendicular point

            var iPoint = GetLineSegmentIntersection(pLine, segment);
            if (iPoint != null)
            {

                return iPoint.Value;
            }
            else
            {

                var distToBegin = GetDistance(point, segment.Begin);
                var distToEnd = GetDistance(point, segment.End);

                return distToBegin < distToEnd ? segment.Begin : segment.End;
            }
        }

        /// <summary>
        /// Возвращает угол, эквивалентный данному, в пределах от [0, 2P)
        /// </summary>
        public static double GetNormalizedAngle(double a)
        {
            return a - Math.Floor(a / (2 * Math.PI)) * 2 * Math.PI;
        }

        /// <summary>
        /// Пересечение луча и окружности. Возвращает массив с 0-2 точками.
        /// </summary>
        public static Vector2[] GetRayCircleIntersection(Ray ray, Circle circle)
        {
            var line = new Line(ray);
            var intersections = GetLineCircleIntersection(line, circle);

            return intersections.Where(p => HasRayPoint(ray, p)).ToArray();
        }

        /// <summary>
        /// Пересечение луча и прямой. НЕ обрабатывает ситуации, когда точек пересечения несчетно много.
        /// </summary>
        public static Vector2? GetRayLineIntersection(Ray ray, Line line)
        {
            var rayLine = new Line(ray.Vector, ray.Begin.X, ray.Begin.Y);
            var intersect = GetLinesIntersection(line, rayLine);

            if (!intersect.HasValue)
                return null;


            var iPoint = intersect.Value;
            if (HasRayPoint(ray, iPoint))
                return iPoint;
            return null;
        }

        /// <summary>
        /// Пересечение луча и отрезка. НЕ обрабатывает ситуации, когда точек несчетно много.
        /// </summary>
        public static Vector2? GetRaySegmentIntersection(Ray ray, Segment segment)
        {
            var sline = new Line(segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);
            var intersect = GetRayLineIntersection(ray, sline);

            if (!intersect.HasValue)
            {
                return null;
            }

            var iPoint = intersect.Value;

            if (HasSegmentPoint(segment, iPoint))
            {
                return iPoint;
            }

            return null;
        }

        public static Vector2? GetSegmentsIntersection(Segment seg1, Segment seg2, bool includeBorders = true)
        {
            var intersection = GetLinesIntersection(new Line(seg1), new Line(seg2));
            if (!intersection.HasValue)
                return null;

            var iPoint = intersection.Value;

            if (double.IsNaN(iPoint.X)) // отрезки лежат на одной прямой
            {
                if (IsSegmentsIntersectionOnLine(seg1, seg2, includeBorders))
                    return iPoint;
            }
            
            if (HasSegmentPoint(seg1, iPoint) && HasSegmentPoint(seg2, iPoint))
            {
                if (!includeBorders)
                    if (seg1.Begin.Equals(iPoint) || seg2.Begin.Equals(iPoint)
                                                  || seg1.End.Equals(iPoint) || seg2.End.Equals(iPoint))
                        return null;

                return iPoint;
            }


            return null;
        }

        /// <summary>
        /// Пересечение отрезков, находящихся на одной прямой.
        /// </summary>
        public static bool IsSegmentsIntersectionOnLine(Segment seg1, Segment seg2, bool includeBorders = true)
        {
            if (seg1.Begin.X.Equal(seg1.End.X)) // если отрезки вертикальны
            {
                var miny2 = Math.Min(seg2.Begin.Y, seg2.End.Y);
                var maxy2 = Math.Max(seg2.Begin.Y, seg2.End.Y);
                var miny1 = Math.Min(seg1.Begin.Y, seg1.End.Y);
                var maxy1 = Math.Max(seg1.Begin.Y, seg1.End.Y);

                if (miny1.Greater(maxy2) || miny2.Greater(maxy1)) // если не пересекается, то null
                    return false;

                if (!includeBorders)
                {
                    if (maxy1.Equal(miny2) || maxy2.Equal(miny1))
                        return false;
                }

                return true;
            }
            else
            {
                var minx2 = Math.Min(seg2.Begin.X, seg2.End.X);
                var maxx2 = Math.Max(seg2.Begin.X, seg2.End.X);
                var minx1 = Math.Min(seg1.Begin.X, seg1.End.X);
                var maxx1 = Math.Max(seg1.Begin.X, seg1.End.X);

                if (minx1.Greater(maxx2) || minx2.Greater(maxx1)) // если не пересекается, то null
                    return false;

                if (!includeBorders)
                {
                    if (maxx1.Equal(minx2) || maxx2.Equal(minx1))
                        return false;
                }

                return true;
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

        public static bool IsSegmentIntersectingRectangles(Segment segment, SimplifiedRectangle[] rectangles, bool includeBorders = true)
        {
            // todo: переделать на простой Rectangle

            foreach (var rect in rectangles)
            {
                Vector2? fpoint = null;

                foreach (var seg in rect.Segments)
                {
                    if (segment.Equals(seg))
                        return true;

                    var inter = GetSegmentsIntersection(seg, segment, true);

                    if (!inter.HasValue)
                        continue;

                    if (fpoint == null)
                    {
                        fpoint = inter;
                        if (includeBorders)
                            return true;
                    }
                    else
                    {
                        if (!fpoint.Value.Equals(inter.Value))
                            return true;
                    }
                }
            }


            return false;
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
    }
}