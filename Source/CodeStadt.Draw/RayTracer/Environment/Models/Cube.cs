using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeStadt.Draw.RayTracer.Environment.Objects;

namespace CodeStadt.Draw.RayTracer.Environment.Models
{
    /// <summary>
    /// Create a model to represent a cube.  Not sure if this is
    /// a good idea, but it'll do for now.
    /// </summary>
    public class Cube : Model
    {
        /// <summary>
        /// Create a new cube
        /// </summary>
        /// <param name="size">The size of the cube</param>
        /// <param name="offset">The offset from the origin of the bottom, left, front corner</param>
        public Cube(double size, Vector offset)
        {
            this.Objects = new List<SceneObject>();

            // Add side 1
            //var side1 = new List<Vector>()
            //{
            //    new Vector(0,0,0) + offset,
            //    new Vector(0,size,0) + offset,
            //    new Vector(size,size,0) + offset,
            //    new Vector(size,0,0) + offset
            //};
            //this.Objects.Add(new Polygon(side1, new Vector(0, 0, -1)) { Surface = Surfaces.White });

            // Add side 2
            //var side2 = new List<Vector>()
            //{
            //    new Vector(0,0,0) + offset,
            //    new Vector(0,size,0) + offset,
            //    new Vector(0,size,size) + offset,
            //    new Vector(0,0,size) + offset
            //};
            //this.Objects.Add(new Polygon(side2, new Vector(-1, 0, 0)) { Surface = Surfaces.White });

            //// Add side 3
            //var side3 = new List<Vector>()
            //{
            //    new Vector(0,0,0) + offset,
            //    new Vector(0,0,size) + offset,
            //    new Vector(size,0,size) + offset,
            //    new Vector(size,0,0) + offset
            //};
            //this.Objects.Add(new Polygon(side3, new Vector(0, -1, 0)) { Surface = Surfaces.White });

            // Add side 4
            var side4 = new List<Vector>()
            {
                new Vector(0,0,size) + offset,
                new Vector(size,0,size) + offset,
                new Vector(size,size,size) + offset,
                new Vector(0,size,size) + offset
            };
            this.Objects.Add(new Polygon(side4, new Vector(0, 0, 1)) { Surface = Surfaces.Red });

            // Add side 5
            var side5 = new List<Vector>()
            {
                new Vector(size,0,0) + offset,
                new Vector(size,size,0) + offset,
                new Vector(size,size,size) + offset,
                new Vector(size,0,size) + offset
            };
            this.Objects.Add(new Polygon(side5, new Vector(1, 0, 0)) { Surface = Surfaces.Red });


            // Add side 6 - top
            var side6 = new List<Vector>()
            {
                new Vector(0,size,0) + offset,
                new Vector(0,size,size) + offset,
                new Vector(size,size,size) + offset,
                new Vector(size,size,0) + offset
            };
            this.Objects.Add(new Polygon(side6, new Vector(0, 1, 0)) { Surface = Surfaces.Red });

        }
    }
}
