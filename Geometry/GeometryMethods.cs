using System;
using System.Linq;

namespace Geometry
{
    public static class GeometryMethods
    {
        /// <summary>
        /// Ищет ближайшую к данной точке точку отрезка
        /// </summary>
        /// <param name="point"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
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
        /// Ищет расстояние от точки до отрезка
        /// </summary>
        /// <param name="point"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static double GetDistanceToSegment(Vector2 point, Segment segment)
        {
            var npoint = GetNearestPointOfSegment(point, segment);
            return GetDistance(point, npoint);
        }

        /// <summary>
        /// Ищет пересечение прямых. НЕ обрабатывает ситуации, когда линии совпадают. Возвращает null, если точки нет
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
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
        /// <param name="line"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
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
        /// Проверяет, лежит ли точка в отрезке. Точка point - точка, лежащая на линии отрезка
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool HasSegmentPoint(Segment segment, Vector2 point)
        {
            var rspoint = segment.Begin.X.Greater(segment.End.X) ? segment.Begin : segment.End; // right segment point
            var lspoint = rspoint.Equals(segment.Begin) ? segment.End : segment.Begin;              // left segment point
            var tspoint = segment.Begin.Y.Greater(segment.End.Y) ? segment.Begin : segment.End; // top segment point
            var bspoint = tspoint.Equals(segment.Begin) ? segment.End : segment.Begin;              // bottom segment point

            return point.X.GreaterOrEqual(lspoint.X) && point.X.LessOrEqual(rspoint.X) &&
                   point.Y.GreaterOrEqual(bspoint.Y) && point.Y.LessOrEqual(tspoint.Y);
        }


        /// <summary>
        /// Пересечение луча и прямой. НЕ обрабатывает ситуации, когда точек пересечения несчетно много.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Vector2? GetRayLineIntersection(Ray ray, Line line)
        {
            var rayLine = new Line(ray.Vector2, ray.Begin.X, ray.Begin.Y);
            var intersect = GetLinesIntersection(line, rayLine);

            if (!intersect.HasValue)
                return null;


            var iPoint = intersect.Value;
            if (HasRayPoint(ray, iPoint))
                return iPoint;
            return null;
        }

        /// <summary>
        /// Проверяет, лежит ли точка на луче. Точка point лежит на линии луча.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool HasRayPoint(Ray ray, Vector2 point)
        {
            if (point.Equals(ray.Begin))
                return true;

            if (!ray.Vector2.X.Equal(0))
            {
                if (ray.Vector2.X.Greater(0))
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
                if (ray.Vector2.Y.Greater(0))
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
        /// Пересечение луча и отрезка. НЕ обрабатывает ситуации, когда точек несчетно много.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Пересечение прямой и окружности. Возвращает массис с 0-2 точками.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="circle"></param>
        /// <returns></returns>
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
        /// Пересечение луча и окружности. Возвращает массив с 0-2 точками.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Vector2[] GetRayCircleIntersection(Ray ray, Circle circle)
        {
            var line = new Line(ray);
            var intersections = GetLineCircleIntersection(line, circle);

            return intersections.Where(p => HasRayPoint(ray, p)).ToArray();
        }

        /// <summary>
        /// Расстояние между двумя точками на плоскости.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double GetDistance(Vector2 v1, Vector2 v2)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }
    }
}