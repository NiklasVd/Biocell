using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public abstract class Entity
    {
        public readonly Transform transform;
        public readonly Renderer renderer;
        // TODO: Implement ID

        public Entity()
        {
            transform = new Transform();
            renderer = new Renderer(transform);
        }

        public virtual void Update(GameTime gameTime)
        {
            transform.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            renderer.Draw(gameTime, spriteBatch);
        }
    }
}
