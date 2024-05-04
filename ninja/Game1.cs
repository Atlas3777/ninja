using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using ninja.Controller;
using Penumbra;

namespace ninja
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneManager sceneManager;
        private PenumbraComponent penumbra;

        GameScene gameScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);
            

            //TODO:Реализовать измениение разрешения 
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sceneManager = new();
        }

        protected override void Initialize()
        {
            base.Initialize();    
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Globals.Content = Content;
            Globals.GraphicsDevice = GraphicsDevice;
            
            sceneManager.AddScane(new GameScene(Content, sceneManager, penumbra));
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Globals.Update(gameTime);

            sceneManager.GetCurrentScene().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            sceneManager.GetCurrentScene().Draw(_spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}