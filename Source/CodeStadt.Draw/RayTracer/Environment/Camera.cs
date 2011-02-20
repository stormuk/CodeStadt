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
        /// 
        /// </summary>
        public Vector Forward { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector Up { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector Right { get; private set; }

        /// <summary>
        /// Create a new instance of the camera class
        /// </summary>
        /// <param name="position">The position of the camera in 3D space</param>
        /// <param name="direction">The direction the camera is pointing</param>
        public Camera(Vector position, Vector direction)
        {
            var down = new Vector(0, -1, 0);
            
            this.Position = position;
            this.Forward = (direction - position).Normalise;   
            this.Right = this.Forward.Cross(down).Normalise.Times(1.5); 
            this.Up = this.Forward.Cross(this.Right).Normalise.Times(1.5); 
        }
    }
}
