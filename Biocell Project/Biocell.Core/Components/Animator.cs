using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Biocell.Core
{
    public sealed class Animator : EntityComponent
    {
        private readonly List<Animation> animations;
        private Animation currentAnimation;
        private Entity myEntity;

        internal Animator(Entity myEntity)
        {
            animations = new List<Animation>();
            this.myEntity = myEntity;
        }

        public void Play(Animation animation)
        {
            if (animations.Contains(animation))
            {
                currentAnimation = animation;
            }
        }
        public void Play(string name)
        {
            var foundAnim = animations.Find(a => a.Name == name);
            if (foundAnim != null)
                Play(foundAnim);
            else
                Debug.LogWarning("The animation \"" + name + "\" does not exist.");
        }

        public void Pause()
        {
            currentAnimation?.Pause(true);
        }
        public void Unpause()
        {
            currentAnimation?.Pause(false);
        }

        public void Stop()
        {
            currentAnimation?.Stop();
            currentAnimation = null;
        }

        public void Add(Animation animation)
        {
            if (!animations.Contains(animation))
            {
                animations.Add(animation);
            }
        }
        public void Remove(Animation animation)
        {
            if (animations.Remove(animation))
            {
                if (currentAnimation == animation)
                {
                    Stop();
                    currentAnimation = null;
                }
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (currentAnimation != null) // = Is playing
            {
                currentAnimation.Update(gameTime);
            }
        }
    }
}
