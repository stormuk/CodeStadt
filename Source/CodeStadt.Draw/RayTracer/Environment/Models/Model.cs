using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeStadt.Draw.RayTracer.Environment.Objects;

namespace CodeStadt.Draw.RayTracer.Environment.Models
{
    public abstract class Model
    {
        public ICollection<SceneObject> Objects { get; set; }
    }
}
