using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tests
{
    [TestClass()]
    public class MeshBuilder3DTests
    {
        [TestMethod()]
        public void PointInTriangleTest1()
        {
            Vector2 point = Vector2.zero;
            Vector2 t0 = new Vector2(-1, -1);
            Vector2 t1 = new Vector2(1, -1);
            Vector2 t2 = new Vector2(0, 10);
            Assert.IsTrue(MeshBuilder3D.PointInTriangle(point, t0, t1, t2));
        }

        [TestMethod()]
        public void PointInTriangleTest2()
        {
            Vector2 point = Vector2.zero;
            Vector2 t0 = new Vector2(-1, 1);
            Vector2 t1 = new Vector2(1, 1);
            Vector2 t2 = new Vector2(0, -10);
            Assert.IsTrue(MeshBuilder3D.PointInTriangle(point, t0, t1, t2));
        }

        [TestMethod()]
        public void PointInTriangleTest3()
        {
            Vector2 point = new Vector2(10,10);
            Vector2 t0 = new Vector2(-1, -1);
            Vector2 t1 = new Vector2(1, -1);
            Vector2 t2 = new Vector2(0, -10);
            Assert.IsFalse(MeshBuilder3D.PointInTriangle(point, t0, t1, t2));
        }

    }
}