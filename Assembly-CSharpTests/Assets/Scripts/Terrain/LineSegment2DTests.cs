using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace Tests
{
    [TestClass()]
    public class LineSegment2DTests
    {
        [TestMethod()]
        public void IntersectsLineTest()
        {
            LineSegment2D lineA = new LineSegment2D(new Vector2(0, 0), new Vector2(1, 1));
            LineSegment2D lineB = new LineSegment2D(new Vector2(1, 0), new Vector2(0, 1));
            Assert.IsTrue(lineA.Intersects(lineB));
        }

        [TestMethod()]
        public void IntersectsPointTest()
        {
            LineSegment2D lineA = new LineSegment2D(new Vector2(0, 0), new Vector2(1, 1));
            Assert.IsTrue(lineA.Intersects(new Vector2(0.5f, 0.5f)));
        }
    }
}