using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace ninja
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;

        List<Sprite> _sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();    
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _sprites = new();

            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Texture2D enemyTexture = Content.Load<Texture2D>("enemy");

            _sprites.Add(new Sprite(enemyTexture, new Vector2(100, 100)));
            _sprites.Add(new Sprite(enemyTexture, new Vector2(400, 200)));
            _sprites.Add(new Sprite(enemyTexture, new Vector2(700, 300)));
            _sprites.Add(new Sprite(enemyTexture, new Vector2(100, 300)));

            player = new Player(playerTexture, new Vector2(200, 200));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            List<Sprite> killList = new();
            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime);

                if (sprite.Rect.Intersects(player.Rect))
                {
                    killList.Add(sprite);
                }
            }

            foreach (var sprite in killList)
            {
                _sprites.Remove(sprite);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }
            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}