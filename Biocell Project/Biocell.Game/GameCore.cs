using Biocell.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Game
{
    public class GameCore
    {
        public const string sceneSavePath = @"Scenes\", sceneFileExtension = ".bcscene",
            resourceComment = "#";
        public static string Version { get { return "1.00"; } }

        private TextureController textures;
        public TextureController Textures { get { return textures; } }

        private GameScene scene;
        public GameScene Scene { get { return scene; } }

        public GameCore(GameScene startScene)
        {
            SetScene(startScene);
        }
        public GameCore()
            : this(new GameScene("Start"))
        {
        }
        public GameCore(string loadSceneName)
            : this(Load(loadSceneName))
        {
        }

        public void SetScene(GameScene newScene)
        {
            scene = newScene;
        }

        public void LoadContent(ContentManager contentManager, string resourceFilePath)
        {
            if (File.Exists(resourceFilePath))
            {
                var textLines = File.ReadAllLines(resourceFilePath).ToList();
                textLines.ForEach(t => t.Trim());
                textLines.RemoveAll(t => t.StartsWith(resourceComment));

                #region Textures
                var textureTextLines = textLines.FindAll(t => t.StartsWith(TextureController.textureResourceHeader)); // Cut the found items out of the textLines list?

                textures = new TextureController(contentManager);
                textures.RegisterTexturesByResourceCode(textureTextLines.ToArray());
                #endregion
            }
        }
        public void UnloadContent()
        {
            if (textures != null)
                textures.UnregisterAll();
        }
        public void Update(GameTime gameTime)
        {
            var updateEntities = scene.entities.ToArray();
            for (int i = 0; i < updateEntities.Length; i++)
            {
                updateEntities[i].Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var drawEntities = scene.entities.ToArray();
            for (int i = 0; i < drawEntities.Length; i++)
            {
                drawEntities[i].Draw(gameTime, spriteBatch);
            }
        }

        private void UnloadResources()
        {
            if (textures != null)
                textures.UnregisterAll();
        }

        public static void Save(GameScene scene)
        {
            var serializedText = JsonConvert.SerializeObject(new SceneSavePackage(Version, scene), Formatting.Indented);
            File.WriteAllText(Path.Combine(sceneSavePath, scene.name, sceneFileExtension), serializedText);
        }
        public static GameScene Load(string name)
        {
            var serializedText = File.ReadAllText(Path.Combine(sceneSavePath, name, sceneFileExtension));
            var sceneSavePackage = JsonConvert.DeserializeObject<SceneSavePackage>(serializedText);

            if (sceneSavePackage.Equals(default(SceneSavePackage)))
            {
                Debug.LogError("Loading the scene " + name + " resulted in an deserialization error.");
                return null;
            }
            if (sceneSavePackage.version != Version)
                Debug.LogWarning("The loaded scene " + name + " has a different version (v" + sceneSavePackage.version + ") " +
                    "than the game core (v" + Version + ")");

            return sceneSavePackage.scene;
        }
    }

    internal struct SceneSavePackage
    {
        public string version;
        public GameScene scene;

        public SceneSavePackage(string version, GameScene scene)
        {
            this.version = version;
            this.scene = scene;
        }

        public override bool Equals(object obj)
        {
            if (obj is SceneSavePackage)
            {
                var sceneSavePackageObj = (SceneSavePackage)obj;
                return version == sceneSavePackageObj.version && scene == sceneSavePackageObj.scene;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
