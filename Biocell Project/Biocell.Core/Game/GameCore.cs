using Biocell.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class GameCore
    {
        public const string sceneSavePath = @"Scenes\", sceneFileExtension = ".bcscene";
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

        public void LoadContent(ContentManager contentManager)
        {
            textures = new TextureController(contentManager);

            #region Cells
            textures.RegisterTexture(@"Textures\Cell1");
            #endregion
        }
        public void UnloadContent()
        {
        }
        public void Update(GameTime gameTime)
        {
            var updateEntities = scene.entities.ToArray();
            for (int i = 0; i < updateEntities.Length; i++)
            {
                if (!updateEntities[i].dontUpdate)
                    updateEntities[i].Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(); // TODO: Set specific settings here

            var drawEntities = scene.entities.ToArray();
            for (int i = 0; i < drawEntities.Length; i++)
            {
                drawEntities[i].Draw(gameTime, spriteBatch);
            }
        }

        private void PerformEntityOptimizations()
        {

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

/* RESOURCE FILE SYSTEM (OLD)

            #This is a comment

            textures:
            Textures\Cell1

            if (File.Exists(resourceFilePath))
            {
                var textLines = File.ReadAllLines(resourceFilePath).ToList();
textLines.ForEach(t => t.Trim()); // Remove spaces from the beginning or end

                // Remove comments
                for (int i = 0; i<textLines.Count; i++)
                {
                    var textLine = textLines[i];
var commentIndex = textLine.IndexOf(resourceCommentOperator);
                    if (commentIndex != -1)
                    {
                        textLines[i] = textLine.Remove(commentIndex, textLine.Length - commentIndex);
                    }
                }

                #region Textures
                var textureTextLines = new List<string>();
// Find out where the first occurance of a "textures" header is
var textureHeaderIndex = textLines.FindIndex(t => t == TextureController.textureResourceHeader + resourceHeaderOperator);
                for (int i = 0; i<textLines.Count - textureHeaderIndex; i++)
                {
                    var textLine = textLines[textureHeaderIndex];

                    if (!textLine.Contains(resourceHeaderOperator)) // If a header operator ":" is found, stop the texture loading
                        break;
                    Debug.Log("Adding texture");
                    // If the texture that is referenced exists, load it into the game
                    if (File.Exists(textLine))
                        textureTextLines.Add(textLine);
                    else continue; // If the texture does not exist, just continue
                }

                textures = new TextureController(contentManager); // Create the texture manager and add all the referenced textures
textures.RegisterTexturesByResourceCode(textureTextLines.ToArray());
                #endregion
            }
*/