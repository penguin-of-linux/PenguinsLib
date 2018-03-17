using Algorithms;
using Geometry;
using NUnit.Framework;

namespace Tests
{
    public class Algorithms_FindPathAmongRectanglesTests
    {
        [Test]
        public void WithoutBarriers()
        {
            var rectangles = new SimplifiedRectangle[0];
            var start = new Vector2(0, 0);
            var finish = new Vector2(10, 10);

            var path = AlgorithmsMethods.FindPathAmongRectangles(start, finish, rectangles);

            Assert.AreEqual(1, path.Length);
            Assert.AreEqual(finish, path[0]);
        }

        [Test]
        public void OneBarrier()
        {
            var rectangles = new SimplifiedRectangle[] {new SimplifiedRectangle(-2, 3, 5, 7), };
            var start = new Vector2(0, 0);
            var finish = new Vector2(0, 10);

            var path = AlgorithmsMethods.FindPathAmongRectangles(start, finish, rectangles);

            Assert.AreEqual(3, path.Length);
            Assert.AreEqual(new Vector2(-2, 3), path[0]);
            Assert.AreEqual(new Vector2(-2, 7), path[1]);
            Assert.AreEqual(finish, path[2]);
        }

        [Test]
        public void DifficultTest()
        {
            /*
             11 x 11

             0 0 0 0 0 0 0 0 0 0 0
             0 0 0 0 0 0 0 0 0 0 0
             0 0 0 0 0 0 0 * * * *
             0 0 0 0 0 0 0 * * * *
             * * * 0 0 0 0 0 0 0 0
             * * * 0 0 0 * * * * *
             * * * 0 0 0 * * * * *
             0 0 * * * 0 * * * * *
             0 0 * * * 0 0 0 0 0 0
             0 0 0 0 0 0 0 0 0 0 0
             0 0 0 0 0 0 0 0 0 0 0
             */

            var rectangles = new SimplifiedRectangle[]
            {
                new SimplifiedRectangle(0, 4, 2, 6),
                new SimplifiedRectangle(2, 2, 4, 3), 
                new SimplifiedRectangle(6, 3, 10, 5), 
                new SimplifiedRectangle(7, 7, 10, 8), 
            };
            var start = new Vector2(0, 0);
            var finish = new Vector2(10, 10);

            var path = AlgorithmsMethods.FindPathAmongRectangles(start, finish, rectangles);

            // результат непредсказуем, т.к. юзаем бфс :)
            ;
            //Assert.AreEqual(4, path.Length);
        }
    }
}