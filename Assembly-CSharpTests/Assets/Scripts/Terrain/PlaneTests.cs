using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tests
{
    [TestClass()]
    public class PlaneTests
    {
        [TestMethod()]
        public void GetMappedPointTest()
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Vector2 mappedPoint = plane.GetMappedPoint(new Vector3(0, 1, 0));
            Vector2 mappedPoint2 = plane.GetMappedPoint(new Vector3(0, 2, 0));
            Assert.IsTrue(Vector2.Distance(mappedPoint, mappedPoint2) < Mathf.Epsilon);
        }

        [TestMethod()]
        public void PlaneDistTest1()
        {
            Plane plane = new Plane(Vector3.up, Vector3.up);
            Assert.AreEqual(plane.GetDistToPlane(Vector3.up * 2), 1);
        }

        [TestMethod()]
        public void PlaneDistTest2()
        {
            Plane plane = new Plane(Vector3.up, Vector3.up);
            Assert.AreEqual(plane.GetDistToPlane(Vector3.zero), -1);
        }

        [TestMethod()]
        public void PlaneDistTest3()
        {
            Plane plane = new Plane(new Vector3(1,2,3), 4);
            float dist = plane.GetDistToPlane(new Vector3(0, 1, 2));
            Assert.IsTrue(Mathf.Abs(dist - 3.20734f) < 0.01f);
        }

        [TestMethod()]
        public void PlaneDistTest4()
        {
            Plane plane = new Plane(new Vector3(1, 2, 3), new Vector3(0, 2, 0));
            Plane reversePlane = new Plane(-plane.normal, -plane.d);
            float dist = reversePlane.GetDistToPlane(new Vector3(0, 1, 2));
            Assert.IsTrue(Mathf.Abs(dist + 1.0690f) < 0.01f);
        }

        [TestMethod()]
        public void PlaneDistTest5()
        {
            Plane plane = new Plane(new Vector3(10000, 20000, 30000), new Vector3(0, 2, 0));
            float dist = plane.GetDistToPlane(new Vector3(0, 1, 2));
            Assert.IsTrue(Mathf.Abs(dist - 1.0690f) < 0.01f);
        }

    }
}