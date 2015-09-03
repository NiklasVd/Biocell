using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class Transform
    {
        public Vector2 position, scale, origin;
        public float rotation;

        public Transform parent;

        // Or the total values?
        internal Vector2 TotalPosition { get { return position + (parent != null ? parent.position : Vector2.Zero); } }
        internal float TotalRotation { get { return rotation + (parent != null ? parent.rotation : 0); } }
        internal Vector2 TotalScale { get { return scale + (parent != null ? parent.scale : Vector2.Zero); } }

        public Transform()
        {
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
