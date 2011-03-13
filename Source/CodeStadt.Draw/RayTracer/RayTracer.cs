namespace CodeStadt.Draw.RayTracer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodeStadt.Draw.RayTracer.Environment;
    using CodeStadt.Draw.RayTracer.Environment.Objects;

    /// <summary>
    /// A ray tracer
    /// </summary>
    /// <remarks>
    /// Inspiration taken from
    /// http://blogs.msdn.com/b/lukeh/archive/2007/04/03/a-ray-tracer-in-c-3-0.aspx?PageIndex=3#comments
    /// </remarks>
    public class RayTracer
    {
        /// <summary>
        /// The number of recursive traces to perform
        /// </summary>
        public int MaxDepth { get; private set; }

        /// <summary>
        /// An action to perform with the result for each pixel
        /// </summary>
        public Action<int, int, System.Drawing.Color> withResult;

        /// <summary>
        /// Create a new instance of the RayTracer class.  Defaults the recursive depth to 5
        /// </summary>
        /// <param name="withResult">The action to perform</param>
        public RayTracer(Action<int, int, System.Drawing.Color> withResult)
            : this(withResult, 5)
        {
        }

        /// <summary>
        /// Create a new instance of the RayTracer class
        /// </summary>
        /// <param name="withResult">The action to perform</param>
        /// <param name="depth">The recursive depth</param>
        public RayTracer(Action<int, int, System.Drawing.Color> withResult, int depth)
        {
            this.withResult = withResult;
            this.MaxDepth = depth;
        }

        /// <summary>
        /// Trace a ray through the scene
        /// </summary>
        /// <param name="ray">The ray to test</param>
        /// <param name="scene">The scene description</param>
        /// <param name="depth">The current depth of recursion</param>
        /// <returns>The color of the pixel</returns>
        private Color TraceRay(Ray ray, Scene scene, int depth)
        {
            var intersection = scene.ClosestIntersection(ray);
            if (intersection == null)
            {
                return Color.Background;
            }

            return this.Shade(intersection, scene, depth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="position"></param>
        /// <param name="norm"></param>
        /// <param name="rayDirection"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        private Color GetNaturalColor(SceneObject element, Vector position, Vector norm, Vector rayDirection, Scene scene)
        {
            var ret = new Color(0, 0, 0);
            foreach (Light light in scene.Lights)
            {
                Vector lightDirection = light.Position - position;
                Vector lightUnitVector = lightDirection.Normalise;
                double intersectionDistance = scene.TestRay(new Ray() { Start = position, Direction = lightUnitVector });
                bool isInShadow = !((intersectionDistance > lightDirection.Magnitude) || (intersectionDistance == 0));

                if (!isInShadow)
                {
                    double angleOfIllumination = Vector.Dot(lightUnitVector, norm);
                    Color lcolor = angleOfIllumination > 0 ? angleOfIllumination * light.Color : new Color(0, 0, 0);
                    double specular = Vector.Dot(lightUnitVector, rayDirection.Normalise);
                    Color scolor = specular > 0 ? Color.Times(Math.Pow(specular, element.Surface.Roughness), light.Color) : new Color(0, 0, 0);
                    ret = ret + (element.Surface.Diffuse(position) * lcolor) + (element.Surface.Specular(position) * scolor);
                }
            }

            return ret;
        }

        /// <summary>
        /// Calculate the color being reflected at a point of intersection
        /// </summary>
        /// <param name="element">The object being intersected</param>
        /// <param name="position">The position on the objects surface</param>
        /// <param name="norm">The normal at the point of intersection</param>
        /// <param name="reflectDirection">The direction of the reflected ray</param>
        /// <param name="scene">The scene description</param>
        /// <param name="depth">The current depth of recursion</param>
        /// <returns>The color being reflected</returns>
        private Color GetReflectionColor(SceneObject element, Vector position, Vector norm, Vector reflectDirection, Scene scene, int depth)
        {
            // Only calculate reflected color if the surface is reflective
            var reflectiveness = element.Surface.Reflectiveness(position);
            if (reflectiveness > 0)
            {
                return reflectiveness * this.TraceRay(new Ray() { Start = position, Direction = reflectDirection }, scene, depth + 1);
            }

            return new Color(0, 0, 0);
        }

        /// <summary>
        /// Get the color of an object at a point of intersection
        /// </summary>
        /// <param name="intersection">The last point of intersection</param>
        /// <param name="scene">The scene description</param>
        /// <param name="depth">The current depth of recursion</param>
        /// <returns>The color of the pixel</returns>
        private Color Shade(Intersection intersection, Scene scene, int depth)
        {
            // Use Fresnel's law to calculate the direction of the reflected light ray
            // R = 2(N.L)*N - L
            // R = reflection direction
            // N = Normal at point of intersection
            // L = -I
            // I = direction of ray
            var direction = -1 * intersection.Ray.Direction;
            var position = (intersection.Distance * intersection.Ray.Direction) + intersection.Ray.Start;
            var normal = intersection.Element.Normal(position);
            var reflectDir = (2 * normal.Dot(direction) * normal) - direction;

            var ret = Color.DefaultColor;
            ret = ret + this.GetNaturalColor(intersection.Element, position, normal, reflectDir, scene);
            if (depth >= MaxDepth)
            {
                return ret + new Color(.5, .5, .5);
            }

            // The color at this point is equal to the color of the object + any color reflecting on to it
            return ret + this.GetReflectionColor(intersection.Element, position + 0.001 * reflectDir, normal, reflectDir, scene, depth);
        }

        

        /// <summary>
        /// Render the scene
        /// </summary>
        /// <param name="scene">The scene to render</param>
        public void Render(Scene scene)
        {
            for (int y = 0; y < scene.Camera.Screen.Height; y++)
            {
                for (int x = 0; x < scene.Camera.Screen.Width; x++)
                {
                    var color = this.TraceRay(new Ray() { Start = scene.Camera.Position, Direction = scene.Camera.GetRayDirection(x, y) }, scene, 0);
                    this.withResult(x, y, color.ToDrawingColor());
                }
            }
        }
    }
}
