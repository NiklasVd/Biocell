using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public sealed class Renderer : EntityComponent
    {
        public Texture2D texture;
        public float layerDepth;

        private readonly Transform usedTransform;

        public Renderer(Transform usedTransform)
        {
            this.usedTransform = usedTransform;
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, position: usedTransform.TotalPosition, origin: usedTransform.origin,
                    rotation: usedTransform.TotalRotation, scale: usedTransform.TotalScale, color: Color.White,
                    effects: SpriteEffects.None, layerDepth: layerDepth);
            }
        }
    }
}
