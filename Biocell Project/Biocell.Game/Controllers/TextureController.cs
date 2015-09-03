using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Game
{
    public class TextureController : IController
    {
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
        public void RegisterTexturesByResourceFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var textLines = File.ReadAllLines(filePath).AsEnumerable();
                Parallel.ForEach(textLines, (t) =>
                {
                    if (File.Exists(t))
                    {
                        RegisterTexture(t);
                    }
                });
            }

            textures.All(k => { Debug.Log(k.Value.Name); return true; });
        }
        public void UnregisterTexture(string path)
        {
            textures.Remove(path);
        }

        public void UnregisterAll()
        {
            // Really do this parallel?
            var texturesCollection = textures.AsEnumerable();
            Parallel.ForEach(texturesCollection, (t) =>
            {
                UnregisterTexture(t.Key);
            });
        }

        private void ProcessResourceFile(string filePath)
        {

        }
    }
}
