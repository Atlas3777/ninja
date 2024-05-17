using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ninja.View.Renderes;


namespace ninja.Controller
{
    public class NinjaGame : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public MonogameRenderer Renderer;

        public double Elapsed;

        public NinjaGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            //TODO:Реализовать измениение разрешения 
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Renderer = new MonogameRenderer();

            SceneManager.Initialize();
            SceneManager.CurrentScene = SceneManager.Scenes["MainMenuScene"];


            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Elapsed = gameTime.ElapsedGameTime.TotalSeconds;

            SceneManager.CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SceneManager.CurrentScene.Draw(Renderer);

            
            base.Draw(gameTime);
        }
    }
}