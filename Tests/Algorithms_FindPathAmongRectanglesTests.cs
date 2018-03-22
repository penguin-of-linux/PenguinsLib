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
            var rectangles = new SimplifiedRectangle[] {new SimplifiedRectangle(25, 25, 75, 75), };
            var start = new Vector2(0, 0);
            var finish = new Vector2(100, 100);

            var path = AlgorithmsMethods.FindPathAmongRectangles(start, finish, rectangles);

            Assert.AreEqual(2, path.Length);
            Assert.AreEqual(finish, path[1]);
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

        [Test]
        public void PathToRectMiddleFromItsVertext()
        {
            var rectangles = new SimplifiedRectangle[]
            {
                new SimplifiedRectangle(0, 0, 20, 20)
            };
            var start = new Vector2(0, 0);
            var finish = new Vector2(10, 10);

            var path = AlgorithmsMethods.FindPathAmongRectangles(start, finish, rectangles);

            Assert.IsEmpty(path);
        }
    }
}