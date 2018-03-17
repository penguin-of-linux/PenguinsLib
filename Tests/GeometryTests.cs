using NUnit.Framework;

using Geometry;

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

        [Test]
        public void Intersection_SameLinesReturns_NaN_and_NaN()
        {
            var line1 = new Line(1, 2, 3);
            var line2 = new Line(2, 4, 6);

            var intersection = GeometryMethods.GetLinesIntersection(line1, line2);

            Assert.True(double.IsNaN(intersection.Value.X));
            Assert.True(double.IsNaN(intersection.Value.Y));
        }
    }
}
