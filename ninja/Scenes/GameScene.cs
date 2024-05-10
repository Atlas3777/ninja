using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ninja.Controller;
using ninja.Model;
using ninja.Scenes;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja
{
    internal class GameScene : IScene
    {
        #region prop
        private readonly ContentManager contentManager;
        private SceneManager sceneManager;
        private Song song;
        private SoundEffect effect;
        private SoundEffectInstance soundEffectInstance;
        public static SpriteFont font;
        //private
        private Player player;
        private Enemy enemy;

        private PenumbraComponent penumbra;
        private RenderTarget2D renderTarget2D;
        

        private Map map;

        public Matrix translate;

        Texture2D background;

        #endregion

        public GameScene(ContentManager contentManager, SceneManager sceneManager, PenumbraComponent penumbra)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.penumbra = penumbra;

            map = new Map(penumbra);
        }

        public void Initialize()
        {

        }

        public void Load()
        {
            song = contentManager.Load<Song>("Audio/fonMusic");
            effect = contentManager.Load<SoundEffect>("Audio/slime_jump");
            font = contentManager.Load<SpriteFont>("Fonts/slimeFont");
            
            var runSpriteSheet = contentManager.Load<Texture2D>("PlayerAnimation/Run");
            var idleSpriteSheet = contentManager.Load<Texture2D>("PlayerAnimation/_Idle");
            var jumpSpriteSheet = contentManager.Load<Texture2D>("PlayerAnimation/_Jump");
            var fallSpriteSheet = contentManager.Load<Texture2D>("PlayerAnimation/_Fall");

            var runAnim = new Animation(runSpriteSheet, 10, 10, new Vector2(120, 80), 2);
            var idleAnim = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));
            var jumpAnim = new Animation(jumpSpriteSheet, 3, 3, new Vector2(120, 80), 3);
            var fallAnim = new Animation(fallSpriteSheet, 3, 3, new Vector2(120, 80), 3);

            var runAnimEnemy = new Animation(runSpriteSheet, 10, 10, new Vector2(120, 80));
            var idleAnimEnemy = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));
            var jumpAnimEnemy = new Animation(jumpSpriteSheet, 3, 3, new Vector2(120, 80), 3);
            var fallAnimEnemy = new Animation(fallSpriteSheet, 3, 3, new Vector2(120, 80), 3);


            player = new Player(runAnim, idleAnim, jumpAnim, fallAnim, map);

            enemy = new Enemy(runAnimEnemy, idleAnimEnemy, jumpAnimEnemy, fallAnimEnemy, map);

            

            map.Load(contentManager);
            map.Initialize(player);
            enemy.Initialize(penumbra);

            background = contentManager.Load<Texture2D>("Background/fon2x1200");
        }
        
        public void Update(GameTime gameTime)
        {
            player.Update();
            enemy.Update();
            map.Update();

            if (enemy.fovRectangle.Intersects(player.bounds))
            {
                player.IsAlive = false;
            }
            else
                player.IsAlive = true;

            CalculateTranslate();
            CalculateTrans();
            penumbra.Transform = translate;


            if (Keyboard.GetState().IsKeyDown(Keys.N))
                sceneManager.AddScane(new ExitScene(contentManager, penumbra));
        }
        private void CalculateTranslate()
        {
            var dx = 1920 / 2 - player.position.X - player.RectangleForDrow.Width / 2;
            dx = MathHelper.Clamp(dx, -map.mapSize.X * map.scaleTM + 1920, 0);
            var dy = 1080 / 2 - player.position.Y - player.RectangleForDrow.Height / 2;
            dy = MathHelper.Clamp(dy, 1000 + map.scaleTM, 0);
            translate = Matrix.CreateTranslation(dx, dy, 0f); 
        }

        Matrix trans;
        private void CalculateTrans()
        {
            
            var dx = 1920 / 2 - player.position.X - player.RectangleForDrow.Width / 2;
            dx = dx * 0.1f;

            dx = MathHelper.Clamp(dx, -map.mapSize.X * map.scaleTM + 1920, 0);

            var dy = 1080 / 2 - (player.position.Y - player.RectangleForDrow.Height / 2) * 0.1f + 540;

            //dy = MathHelper.Clamp(dy, 1080, 1200);
            dy = MathHelper.Clamp(dy, 1080, 0);

            trans = Matrix.CreateTranslation(dx, dy, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //penumbra.BeginDraw();

            Globals.GraphicsDevice.Clear(Color.DarkCyan);
            spriteBatch.Begin( transformMatrix: trans);
            DrowBackground(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translate, sortMode: SpriteSortMode.BackToFront);

            map.Drow(spriteBatch);
            player.Drow(spriteBatch);
            enemy.Drow(spriteBatch);
            
            spriteBatch.End();

            //penumbra.Draw(gameTime);

            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);


            spriteBatch.DrawString
                (font,
                $" X: {player.position.X} \n Y: {player.position.Y} \n onGround: {player.onGround} \n  {player.velocity} \n {player.IsAlive} \n {player.jumpForse}",
                new Vector2(50, 50), Color.White, 0, Vector2.Zero, 1, 0, 1);

            spriteBatch.End();

        }

        private void DrowBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                background,
                new Rectangle(0, -1200, 3840, 1200),
                null,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

    }
}
