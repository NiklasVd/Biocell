using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class TextureController : IController
    {
        public const string textureResourceHeader = "textures";

        private readonly Dictionary<string, Texture2D> textures;
        private readonly ContentManager contentManager;

        public Texture2D this[string pathKey]
        {
            get { return textures[pathKey]; }
        }

        public TextureController(ContentManager contentManager)
        {
            textures = new Dictionary<string, Texture2D>();
            this.contentManager = contentManager;
        }

        public void RegisterTexture(string path)
        {
            var texture = contentManager.Load<Texture2D>(path);
            textures.Add(path, texture);
        }
        public void RegisterTexturesByResourceCode(string[] textLines)
        {
            foreach (var textLine in textLines)
            {
                RegisterTexture(textLine);
                Debug.Log("Resource: " + textLine);
            }
        }

        private void ProcessResourceFile(string filePath)
        {

        }
    }
}
