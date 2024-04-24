using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace ninja
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneManager sceneManager;
        private FollowCamera camera;


        Animation animationManager;
       
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //TODO:Реализовать измениение разрешения 
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sceneManager = new();

            camera = new FollowCamera(Vector2.Zero);
        }

        

        protected override void Initialize()
        {
            base.Initialize();    
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            sceneManager.AddScane(new GameScene(Content, sceneManager));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //camera.Follow(pla)

            sceneManager.GetCurrentScene().Update(gameTime);

            //animationManager.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            sceneManager.GetCurrentScene().Draw(_spriteBatch);

            //animationManager.Drow(_spriteBatch, spriteSheet);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}