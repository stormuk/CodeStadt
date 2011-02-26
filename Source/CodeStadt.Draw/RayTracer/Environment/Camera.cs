namespace CodeStadt.Draw.RayTracer.Environment
{
    /// <summary>
    /// Represent the camera used to view the scene
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The position of the camera
        /// </summary>
        public Vector Position { get; private set; }

        /// <summary>
        /// Vector pointing forwards from the camera
        /// </summary>
        public Vector Forward { get; private set; }

        /// <summary>
        /// Vector pointing perpendicular and right of the camera
        /// </summary>
        public Vector Up { get; private set; }

        /// <summary>
        /// Vector pointing perpendicular and up from the camera
        /// </summary>
        public Vector Right { get; private set; }

        /// <summary>
        /// Create a new instance of the camera class
        /// </summary>
        /// <param name="position">The position of the camera in 3D space</param>
        /// <param name="direction">The direction the camera is pointing</param>
        public Camera(Vector position, Vector direction)
        {
            // Remember: The cross of two vectors gives you a normal vector out of the cross
            var down = new Vector(0, -1, 0);
            
            this.Position = position;
            this.Forward = (direction - position).Normalise;   
            this.Right = this.Forward.Cross(down).Normalise; 
            this.Up = this.Forward.Cross(this.Right).Normalise;
        }
    }
}
