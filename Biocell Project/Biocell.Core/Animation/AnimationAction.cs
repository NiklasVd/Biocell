using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public delegate void OnAnimationActionTriggeredHandler(AnimationAction action, float totalTimeSeconds);

    public class AnimationAction
    {
        public readonly Vector2 relativePosition;
        public readonly float relativeRotation;
        public readonly Vector2 relativeScale;

        public readonly float timestepSeconds;
        public OnAnimationActionTriggeredHandler onAnimationTriggered;

        public AnimationAction(float timestepSeconds, Vector2 relativePosition = default(Vector2), float relativeRotation = 0, Vector2 relativeScale = default(Vector2), OnAnimationActionTriggeredHandler onAnimationTriggered = null)
        {
            this.relativePosition = relativePosition;
            this.relativeRotation = relativeRotation;
            this.relativeScale = relativeScale;

            this.timestepSeconds = timestepSeconds;
            this.onAnimationTriggered = onAnimationTriggered;
        }
    }
}
