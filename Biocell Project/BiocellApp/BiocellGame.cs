using Biocell.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiocellApp
{
    public class BiocellGame : Game
    {
        private GameCore gameCore;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public BiocellGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            gameCore = new GameCore();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameCore.LoadContent(Content, @"Content\Content Resource Keys.txt");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            gameCore.UnloadContent();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            gameCore.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PeachPuff);
            gameCore.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
