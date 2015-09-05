using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class AnimationAction
    {
        public readonly Vector2 relativePosition;
        public readonly float relativeRotation;
        public readonly Vector2 relativeScale;

        public readonly float timestepSeconds;

        public AnimationAction(float timestepSeconds, Vector2 relativePosition = default(Vector2), float relativeRotation = 0, Vector2 relativeScale = default(Vector2))
        {
            this.relativePosition = relativePosition;
            this.relativeRotation = relativeRotation;
            this.relativeScale = relativeScale;

            this.timestepSeconds = timestepSeconds;
        }
    }
}
