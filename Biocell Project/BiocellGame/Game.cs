using Biocell.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BiocellGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public GameCore gameCore;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private EukaryoteCell cell;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameCore = new GameCore();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //gameCore.LoadContent(Content, @"Content\Resource Keys.txt");

            var texture = Content.Load<Texture2D>(@"Textures\Cell1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            gameCore.UnloadContent();
        }

        bool once;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameCore.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !once)
            {
                once = true;

                cell = new EukaryoteCell();
                cell.transform.position = new Vector2(30, 30);
                cell.renderer.texture = gameCore.Textures[@"Textures\Cell1"];

                cell.animator.Add(new Animation("Idle")
                    .Move(new Vector2(0, 5), 2)
                    .Move(new Vector2(0, -10), 2));

                gameCore.Scene.AddEntity(cell);
                cell.animator.Play("Idle");
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameCore.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
