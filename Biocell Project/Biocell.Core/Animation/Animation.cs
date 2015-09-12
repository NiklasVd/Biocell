using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public sealed class Animation
    {
        // TODO: Implement visibility play modifier so that if the entity is not in the view of the player camera the animation gets paused if currently playing?
        public float playSpeed;
        public bool playBackwards;
        
        internal readonly List<AnimationAction> actions;

        internal AnimationAction currentAction;
        internal AnimationAction nextAction;

        private bool isPlaying;
        public bool IsPlaying { get { return isPlaying; } }

        private bool isPaused;
        public bool IsPaused { get { return isPaused; } }

        private float currentPlayTimeSeconds;
        public float CurrentPlayTimeSeconds { get { return currentPlayTimeSeconds; } }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Entity target;
        private int currentActionIndex;

        public Animation(string name)
        {
            actions = new List<AnimationAction>();
            this.name = name;
        }

        public Animation Move(Vector2 relativePosition, float timestepSeconds = 1)
        {
            return Put(new AnimationAction(timestepSeconds, relativePosition: relativePosition));
        }
        public Animation Rotate(float rotation, float timestepSeconds = 1)
        {
            return Put(new AnimationAction(timestepSeconds, relativeRotation: rotation));
        }
        public Animation Scale(Vector2 relativeScale, float timestepSeconds = 1)
        {
            return Put(new AnimationAction(timestepSeconds, relativeScale));
        }
        public Animation Wait(float timestepSeconds = 1)
        {
            return Put(new AnimationAction(timestepSeconds));
        }
        public Animation AppendTrigger(OnAnimationActionTriggeredHandler onAnimationActionTriggered)
        {
            var lastAction = actions.LastOrDefault();
            if (lastAction != default(AnimationAction))
                lastAction.onAnimationTriggered = onAnimationActionTriggered;

            return this;
        }

        public Animation Put(AnimationAction action)
        {
            actions.Add(action);
            return this;
        }

        internal void Play(Entity target)
        {
            this.target = target;
            currentActionIndex = playBackwards ? actions.Count - 1 : 0;

            isPlaying = true;

        }
        internal void Pause(bool state)
        {
            if (isPlaying)
            {
                isPaused = state;
            }
        }
        internal void Stop()
        {
            isPlaying = false;
            ResetActionMetadata();
        }

        internal void Update(GameTime gameTime)
        {
            if (isPlaying && !isPaused)
            {
                var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds * playSpeed;
                currentPlayTimeSeconds += elapsedTime;

                var pos = target.transform.TotalPosition;
                var rot = target.transform.TotalRotation;
                var scl = target.transform.TotalScale;

                target.transform.position = Vector2.Lerp(pos, currentAction.relativePosition, elapsedTime);
                target.transform.rotation = MathHelper.Lerp(rot, rot + currentAction.relativeRotation, elapsedTime);
                target.transform.scale = Vector2.Lerp(scl, scl + currentAction.relativeScale, elapsedTime);

                if (currentPlayTimeSeconds > currentAction.timestepSeconds)
                    SetNextAction();
            }
        }

        private void ResetActionMetadata()
        {
            target = null;

            currentActionIndex = 0;
            currentAction = default(AnimationAction);
            nextAction = default(AnimationAction);
        }
        private void SetNextAction()
        {
            currentAction = nextAction;

            if (!playBackwards) // Is this really the best way?
            {
                if (actions.Last() != currentAction)
                {
                    currentActionIndex += 1;
                    nextAction = actions[currentActionIndex + 1];
                }
            }
            else
            {
                if (actions.First() != currentAction)
                {
                    currentActionIndex -= 1;
                    nextAction = actions[currentActionIndex - 1];
                }
            }
        }
    }
}
