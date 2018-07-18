using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

using Geometry;
using NUnit.Framework.Internal;

namespace Tests
{
    public class GeometryTests
    {
        // Замечание: в большинстве тестов на пересечение проверяется только тип возвращаемого объекта, 
        //т.к. все пересечения основаны на пересечении прямых. Пересечение прямых и отрезков проверяется нормально.

        [TestCase(0, 0, -1, 1, 2, 1, 0, 1, TestName = "Simple perpendicular")]
        [TestCase(0, 0, 1, 1, 2, 1, 1, 1, TestName = "Left seg point")]
        [TestCase(0, 0, -2, 1, 0, 1, 0, 1, TestName = "Right seg point")]
        [TestCase(0, 0, -1, 0, 1, 0, 0, 0, TestName = "On segment")]
        [TestCase(0, 0, 1, 0, 2, 0, 1, 0, TestName = "On seg line")]
        public void GetNearestPointOfSegment_TestCase(double xp, double yp, double xs1, double ys1, 
            double xs2, double ys2, double expx, double expy)
        {
            var point = new Vector2(xp, yp);
            var seg = new Segment(xs1, ys1, xs2, ys2);
            var expPoint = new Vector2(expx, expy);

            var intPoint = GeometryMethods.GetNearestPointOfSegment(point, seg);

            Assert.True(intPoint.Equals(expPoint));
        }

        [TestCase(0, 0, 2, 2, 1, 1, 3, 3, ExpectedResult = true, TestName = "Simple 1")]
        [TestCase(2, 2, 0, 0, 3, 3, 1, 1, ExpectedResult = true, TestName = "Somple 2")]
        [TestCase(0, 0, 2, 2, 2, 0, 3, 2, ExpectedResult = true, TestName = "Common side")]
        [TestCase(0, 0, 2, 2, 2, 2, 3, 3, ExpectedResult = true, TestName = "Common vertex")]
        [TestCase(0, 0, 3, 3, 1, 1, 2, 2, ExpectedResult = true, TestName = "Inserted")]
        [TestCase(0, 0, 0, 10, -1, 3, 1, 7, ExpectedResult = true, TestName = "One rect is segment! Proizvol! Hotya it works...")]
        [TestCase(0, 0, 2, 2, 2, 0, 2, 2, ExpectedResult = true, TestName = "Another rect is segment")]
        [TestCase(0, 0, 1, 1, 2, 2, 3, 3, ExpectedResult = false, TestName = "Simple false 1")]
        [TestCase(1, 1, 0, 0, 3, 3, 2, 2, ExpectedResult = false, TestName = "Simple false 2")]
        public bool IsSimplifiedRectanglesInterescting_TestCase(double x1, double y1, double x2, double y2, 
            double x3, double y3, double x4, double y4)
        {
            var rect1 = new SimplifiedRectangle(x1, y1, x2, y2);
            var rect2 = new SimplifiedRectangle(x3, y3, x4, y4);

            return GeometryMethods.IsSimplifiedRectanglesInterescting(rect1, rect2);
        }

        [TestCase(0, 0, 0, 2, -1, 1, 1, 1, ExpectedResult = IntersectionResult.Point, TestName = "Simple '+'")]
        [TestCase(0, 0, 2, 0, 1, 0, 3, 0, ExpectedResult = IntersectionResult.Segment, TestName = "One common line")]
        [TestCase(0, 0, 2, 0, 2, 0, 2, 2, ExpectedResult = IntersectionResult.Point, TestName = "One common vertex, on line")]
        [TestCase(0, 0, 0, 2, 0, 2, 3, 3, ExpectedResult = IntersectionResult.Point, TestName = "One common vertex, off line")]
        [TestCase(0, 0, 2, 0, 2, 2, 4, 2, ExpectedResult = IntersectionResult.None, TestName = "Parallel OX")]
        [TestCase(0, 0, 0, 2, 1, 0, 1, 2, ExpectedResult = IntersectionResult.None, TestName = "Parallel OY")]
        [TestCase(0, 2, 2, 0, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Point, TestName = "No simple '+'")]
        [TestCase(0, 1, 1, 2, 0, 0, 42, -42, ExpectedResult = IntersectionResult.None, TestName = "Just NONE")]
        public IntersectionResult GetSegmentsIntersection_TestCase(double x1, double y1, double x2, double y2, 
            double x3, double y3, double x4, double y4)
        {
            var seg1 = new Segment(x1, y1, x2, y2);
            var seg2 = new Segment(x3, y3, x4, y4);

            return GeometryMethods.GetSegmentsIntersection(seg1, seg2, out var result);
        }

        [TestCase(0, 0, 1, 0, 0, 1, 0, 0, ExpectedResult = IntersectionResult.Point, TestName = "+")]
        [TestCase(0, 0, 1, 1, 0, 1, 1, 0, ExpectedResult = IntersectionResult.Point, TestName = "x")]
        [TestCase(0, 0, 1, 0, 0, 1, 1, 1, ExpectedResult = IntersectionResult.None, TestName = "===")]
        [TestCase(0, 0, 1, 0, -1, 0, 3, 0, ExpectedResult = IntersectionResult.Line, TestName = "----")]
        [TestCase(0, 0, 0, 1, 1, 0, 1, 1, ExpectedResult = IntersectionResult.None, TestName = "||")]
        [TestCase(0, 0, 0, 1, 0, 4, 0, 10, ExpectedResult = IntersectionResult.Line, TestName = "|")]
        [TestCase(0, 0, 1, 1, 1, 0, 2, 1, ExpectedResult = IntersectionResult.None, TestName = "//")]
        public IntersectionResult GetLinesIntersection_Type_TestCase(double x1, double y1, double x2, double y2, 
            double x3, double y3, double x4, double y4)
        {
            var line1 = new Line(x1, y1, x2, y2);
            var line2 = new Line(x3, y3, x4, y4);

            return GeometryMethods.GetLinesIntersection(line1, line2, out var result);
        }

        [TestCase(0, 0, 1, 0, 0, 1, 0, 0, 0, 0, TestName = "+")]
        [TestCase(0, 0, 1, 1, -5, 3, -4, 2, -1, -1, TestName = @"\/")]
        public void GetLinesIntersection_Value_TestCase(double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4, double expx, double expy)
        {
            var line1 = new Line(x1, y1, x2, y2);
            var line2 = new Line(x3, y3, x4, y4);

            GeometryMethods.GetLinesIntersection(line1, line2, out var result);

            Assert.AreEqual(new Vector2(expx, expy), (Vector2)result);
        }

        [TestCase(1, 0, 1, 0, ExpectedResult = true, TestName = "(1, 0) (1, 0)")]
        [TestCase(1, 0, 2, 0, ExpectedResult = true, TestName = "(1, 0) (2, 0)")]
        [TestCase(1, 0, 0, 1, ExpectedResult = false, TestName = "(1, 0) (0, 1)")]
        public bool IsVectorCollinear_TestCase(double x1, double y1, double x2, double y2)
        {
            var v1 = new Vector2(x1, y1);
            var v2 = new Vector2(x2, y2);

            return GeometryMethods.IsVectorsCollinear(v1, v2);
        }

        [TestCase(false, 0, 2, 2, 2, 0, 0, 1, 1, ExpectedResult = IntersectionResult.None, TestName = "F, Not intersecting")]
        [TestCase(true, 0, 2, 2, 2, 0, 0, 1, 1, ExpectedResult = IntersectionResult.None, TestName = "T, Not intersecting")]
        [TestCase(false, -1, 0, 1, 2, 0, 0, 1, 1, ExpectedResult = IntersectionResult.Point, TestName = "F, Rect vertex")]
        [TestCase(true, -1, 0, 1, 2, 0, 0, 1, 1, ExpectedResult = IntersectionResult.Point, TestName = "T, Rect vertex")]
        [TestCase(false, 1, 3, 1, 1, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Point, TestName = "F, One side inter")]
        [TestCase(true, 1, 3, 1, 1, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Segment, TestName = "T, One side inter")]
        [TestCase(false, 1, 2, 1, 1, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Point, TestName = "F, Seg begin on rect side")]
        [TestCase(true, 1, 3, 1, 1, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Segment, TestName = "T, Seg begin on rect side")]
        [TestCase(false, 1, 1, 2, 1, 0, 0, 3, 3, ExpectedResult = IntersectionResult.None, TestName = "F, Seg full inside rect")]
        [TestCase(true, 1, 1, 2, 1, 0, 0, 3, 3, ExpectedResult = IntersectionResult.Segment, TestName = "T, Seg full inside rect")]
        [TestCase(false, 0, 0, 2, 2, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Points, TestName = "F, Seg vertexes on rect")]
        [TestCase(true, 0, 0, 2, 2, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Segment, TestName = "T, Seg vertexes on rect")]
        [TestCase(false, 0, 0, 2, 0, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Segment, TestName = "F, Seg on rect side")]
        [TestCase(true, 0, 0, 2, 0, 0, 0, 2, 2, ExpectedResult = IntersectionResult.Segment, TestName = "T, Seg on rect side")]

        [TestCase(true, 0, 0, 75, 75, 25, 25, 75, 75, ExpectedResult = IntersectionResult.Segment, TestName = "T, LALALA")]
        public IntersectionResult GetSegmentSimplifiedRectangleIntersection_TestCase(bool filled, params double[] nums)
        {
            var seg = new Segment(nums[0], nums[1], nums[2], nums[3]);
            var rect = new SimplifiedRectangle(nums[4], nums[5], nums[6], nums[7]);

            return GeometryMethods.GetSegmentSimplifiedRectangleIntersection(seg, rect, out var result, filled);
        }

        [TestCase(Math.PI, Math.PI, TestName = "P -> P")]
        [TestCase(3 * Math.PI, Math.PI, TestName = "3P -> P")]
        [TestCase(-3 * Math.PI, Math.PI, TestName = "-3P -> P")]
        [TestCase(3 * Math.PI + Math.PI / 2, 3 * Math.PI / 2, TestName = "3.5P -> 1.5P")]
        [TestCase(-3 * Math.PI - Math.PI / 2, Math.PI / 2, TestName = "-3.5P -> P/2")]
        [TestCase(1, 1, TestName = "1 -> 1")]
        public void GetNormalizedAngle_TestCase(double angle, double expected)
        {
            var result = GeometryMethods.GetNormalizedAngle(angle);

            Assert.AreEqual(expected, result, 0.01);
        }

        [TestCase(0, Math.PI, Math.PI, TestName = "0, P")]
        [TestCase(0, Math.PI * 3/2d, -Math.PI / 2, TestName = "0, 3/2P")]
        [TestCase(Math.PI, Math.PI * 3/2d, Math.PI / 2, TestName = "P, 3/2P")]
        [TestCase(Math.PI * 3/2d, Math.PI, -Math.PI / 2, TestName = "3/2P, P")]
        [TestCase(1, 1, 0, TestName = "1, 1")]
        [TestCase(Math.PI / 4, -Math.PI / 4, -Math.PI / 2, TestName = "P/4, -P/4")]
        public void GetAnglesDifference_TestCase(double a1, double a2, double expected)
        {
            var result = GeometryMethods.GetAnglesDifference(a1, a2);

            Assert.AreEqual(expected, result, 0.01);
        }

        [TestCase(1, 0, 1, 1, Math.PI / 4, TestName = "(1, 0), (1, 1), P/4")]
        [TestCase(1, 1, 1, 0, 2 * Math.PI - Math.PI / 4, TestName = "(1, 1), (1, 0), 7P/4")]
        public void GetAngleBetweenVectorCww_TestCase(double x1, double y1, double x2, double y2, double exp)
        {
            var v1 = new Vector2(x1, y1);
            var v2 = new Vector2(x2, y2);

            var result = GeometryMethods.GetAngleBetweenVectorCww(v1, v2);

            Assert.AreEqual(exp, result, 0.01);
        }

        [TestCase(0, 0, 1, 1, 0, 0, 3, ExpectedResult = IntersectionResult.Point, TestName = "Circle center is ray begin")]
        public IntersectionResult GetRayCircleIntersection(double x1, double y1, double x2, double y2, double xc, double yc, double r)
        {
            var ray = new Ray(new Vector2(x1, y1), new Vector2(x2, y2));
            var circle = new Circle(xc, yc, r);

            return GeometryMethods.GetRayCircleIntersection(ray, circle, out var _);
        }

        [Test]
        public void GetLineCircleIntersection_NoPoints()
        {
            var line = new Line(0, 2, 1, 2);
            var circle = new Circle(0, 0, 1);

            var inter = GeometryMethods.GetLineCircleIntersection(line, circle, out var result);

            Assert.AreEqual(IntersectionResult.None, inter);
        }

        [Test]
        public void GetLineCircleIntersection_OnePoint()
        {
            var line = new Line(0, 1, 1, 1);
            var circle = new Circle(0, 0, 1);

            var inter = GeometryMethods.GetLineCircleIntersection(line, circle, out var result);

            Assert.AreEqual(new Vector2(0, 1), (Vector2) result);
        }
    }
}
