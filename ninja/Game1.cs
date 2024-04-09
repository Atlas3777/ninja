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

        //MovingSprite sprite;
        //List<MovingSprite> movingsprites;

        //bool space_pressed = false; 

        List<Sprite> _sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _sprites = new();

            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Texture2D enemyTexture = Content.Load<Texture2D>("player");


            _sprites.Add(new Sprite(enemyTexture, new Vector2(100, 100)));
            _sprites.Add(new Sprite(enemyTexture, new Vector2(400, 200)));
            _sprites.Add(new Sprite(enemyTexture, new Vector2(700, 300)));

            _sprites.Add(new Player(playerTexture, new Vector2(200, 200)));

            //sprite = new MovingSprite(texture, Vector2.Zero, 1f);

            //movingsprites = new List<MovingSprite>();
            //for (int i = 0; i < 10; i++)
            //{
            //    movingsprites.Add(new MovingSprite(texture, new Vector2(0,i*100), i));
            //}
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //sprite.Update();

            //foreach (MovingSprite movingsprite in movingsprites)
            //{
            //    movingsprite.Update();
            //}


            //if (!space_pressed && Keyboard.GetState().IsKeyDown(Keys.Space))
            //{
            //    space_pressed = true;
            //    Debug.WriteLine("Space key pressed!");
            //}

            //if (Keyboard.GetState().IsKeyUp(Keys.Space))
            //{
            //    space_pressed = false;
            //}


            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    Debug.WriteLine("Нажата левая кнопка мыши");
            //}

            //var mouseX = Mouse.GetState().X;

            //if (true)
            //{
            //    Debug.WriteLine(mouseX);
            //}

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            ////_spriteBatch.Draw(sprite.texture, sprite.Rect, Color.White);
            //foreach (MovingSprite movingsprite in movingsprites)
            //{
            //    _spriteBatch.Draw(movingsprite.texture, movingsprite.Rect, Color.White);
            //}

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}