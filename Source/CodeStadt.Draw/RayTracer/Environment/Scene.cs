namespace CodeStadt.Draw.RayTracer.Environment
{
    using System.Collections.Generic;
    using System.Linq;
    using CodeStadt.Draw.RayTracer.Environment.Objects;

    /// <summary>
    /// Represent the scene: objects, lighting and camera.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// The objects in the scene
        /// </summary>
        public SceneObject[] Elements { get; set; }

        /// <summary>
        /// The lights in the world
        /// </summary>
        public Light[] Lights { get; set; }

        /// <summary>
        /// The camera position
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Perform an intersection test between a ray and the elements in the scene
        /// </summary>
        /// <param name="r">The ray to test</param>
        /// <returns>The intersection points</returns>
        public IEnumerable<Intersection> Intersect(Ray r)
        {
            return from el in Elements
                   select el.Intersect(r);
        }
    }
}
