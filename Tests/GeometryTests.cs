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

        [TestCase(0, 0, 0, 2, -1, 1, 1, 1, true, ExpectedResult = true, TestName = "Simple '+'")]
        [TestCase(0, 0, 2, 0, 1, 0, 3, 0, true, ExpectedResult = true, TestName = "One common line")]
        [TestCase(0, 0, 2, 0, 2, 0, 2, 2, true, ExpectedResult = true, TestName = "One common vertex")]
        [TestCase(0, 0, 2, 0, 1, 1, 1, -1, false, ExpectedResult = true, TestName = "Include borders = false, result = true")]
        [TestCase(0, 0, 2, 0, 2, 2, 4, 2, true, ExpectedResult = false, TestName = "Parallel")]
        [TestCase(-10, -10, 0, -10, 5, 5, 3, 3, true, ExpectedResult = false, TestName = "Random numbers :)")]
        [TestCase(0, 0, 2, 0, 2, 0, 3, 0, false, ExpectedResult = false, TestName = "Include borders = false, result = false")]
        public bool GetSegmentsIntersection_TestCase(double x1, double y1, double x2, double y2, 
            double x3, double y3, double x4, double y4, bool includeBorders)
        {
            var seg1 = new Segment(x1, y1, x2, y2);
            var seg2 = new Segment(x3, y3, x4, y4);

            return GeometryMethods.GetSegmentsIntersection(seg1, seg2, includeBorders).HasValue;
        }

        [TestCase(0, 0, 100, 100, false, 25, 25, 75, 75, ExpectedResult = true, TestName = "Simple diagonal, include = false")]
        [TestCase(0, 0, 100, 100, false, 1, 1, 99, 10, ExpectedResult = true, TestName = "Simple, include = false")]
        [TestCase(0, 0, 100, 100, true, 25, 25, 75, 75, ExpectedResult = true, TestName = "Simple diagonal, include = true")]
        [TestCase(0, 0, 100, 100, true, 1, 1, 99, 10, ExpectedResult = true, TestName = "Simple, include = true")]
        [TestCase(0, 0, 100, 100, true, 98, 10, 99, 12, ExpectedResult = false, TestName = "Simple, false, include = true")]
        [TestCase(0, 0, 100, 100, false, 25, 25, 50, 10, ExpectedResult = false, TestName = "One point, include = false")]
        [TestCase(0, 0, 100, 100, true, 25, 25, 50, 10, ExpectedResult = true, TestName = "One point, include = true")]
        public bool IsSegmentIntersectingRectangles_TestCase(double x1, double y1, double x2, double y2,
            bool includeBorders, params double[] numbers)
        {
            var seg = new Segment(new Vector2(x1, y1), new Vector2(x2, y2));
            var rectangles = new List<SimplifiedRectangle>();
            for(int i = 0; i < numbers.Length; i += 4)
                rectangles.Add(new SimplifiedRectangle(numbers[i], numbers[i + 1], numbers[i + 2], numbers[i + 3]));

            return GeometryMethods.IsSegmentIntersectingRectangles(seg, rectangles.ToArray(), includeBorders);
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

        [Test]
        public void Intersection_SameLinesReturns_NaN_and_NaN()
        {
            var line1 = new Line(1, 2, 3);
            var line2 = new Line(2, 4, 6);

            var intersection = GeometryMethods.GetLinesIntersection(line1, line2);

            Assert.True(double.IsNaN(intersection.Value.X));
            Assert.True(double.IsNaN(intersection.Value.Y));
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
    }
}
