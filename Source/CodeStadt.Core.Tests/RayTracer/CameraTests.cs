using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CodeStadt.Draw.RayTracer;
using CodeStadt.Draw.RayTracer.Environment;

namespace CodeStadt.Core.Tests.RayTracer
{
    [TestFixture]
    public class CameraTests
    {
        [Test]
        public void CanConstructNewCamera()
        {
            // Arrange
            var pos = new Vector(0, 0, 0);
            var dir = new Vector(0, 0, 1);

            // Act
            var camera = new Camera(pos, dir);

            // Assert
            Assert.AreEqual(pos, camera.Position);

            // Forward is a unit vector pointing in 'direction' from 'position'
            Assert.IsTrue(camera.Forward.Equals(new Vector(0, 0, 1)));

            // Right
            Assert.IsTrue(camera.Right.Equals(new Vector(1,0,0)));

            // Up
            Assert.IsTrue(camera.Up.Equals(new Vector(0,1,0)));
        }
    }
}
