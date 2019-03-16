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
    }
}