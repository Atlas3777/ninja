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
        private Texture2D runSpriteSheet;
        private Texture2D idleSpriteSheet;

        private PenumbraComponent penumbra;
        private RenderTarget2D renderTarget2D;
        

        private Map map;

        public Matrix translate;

        #endregion

        public GameScene(ContentManager contentManager, SceneManager sceneManager, PenumbraComponent penumbra)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.penumbra = penumbra;

            map = new Map(penumbra);
        }

        
        public void Load()
        {
            song = contentManager.Load<Song>("Audio/fonMusic");
            effect = contentManager.Load<SoundEffect>("Audio/slime_jump");
            font = contentManager.Load<SpriteFont>("Fonts/slimeFont");
            
            runSpriteSheet = contentManager.Load<Texture2D>("_Run");
            idleSpriteSheet = contentManager.Load<Texture2D>("_Idle");
            var jumpSpriteSheet = contentManager.Load<Texture2D>("_Jump");
            var fallSpriteSheet = contentManager.Load<Texture2D>("_Fall");

            var runAnim = new Animation(runSpriteSheet, 10, 10, new Vector2(120, 80), 2);
            var idleAnim = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));
            var jumpAnim = new Animation(jumpSpriteSheet, 3, 3, new Vector2(120, 80), 3);
            var fallAnim = new Animation(fallSpriteSheet, 3, 3, new Vector2(120, 80), 3);

            var runAnimEnemy = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));
            var idleAnimEnemy = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));

            var collisions = map.GetCollisionRect();
            //collisions.Add(new Rectangle(200, -200, 200, 50));

            player = new Player(runAnim, idleAnim, jumpAnim, fallAnim, map);
            //enemy = new Enemy(runAnimEnemy, idleAnimEnemy, collisions);

            //enemy.position = new Vector2(400, 400);

            map.Load(contentManager);
            map.Initialize(player);

            
        }
        
        public void Update(GameTime gameTime)
        {
            player.Update();
            //enemy.Update();
            map.Update();

            CalculateTranslate();
            penumbra.Transform = translate;


            if (Keyboard.GetState().IsKeyDown(Keys.N))
                sceneManager.AddScane(new ExitScene(contentManager, penumbra));
        }
        private void CalculateTranslate()
        {
            var dx = 1920 / 2 - player.position.X - player.PlayerRectangle.Width / 2;
            dx = MathHelper.Clamp(dx, -map.mapSize.X * map.scaleTM + 1920, 0);
            var dy = 1080 / 2 - player.position.Y - player.PlayerRectangle.Height / 2;
            dy = MathHelper.Clamp(dy, 1000 + map.scaleTM, 0);
            translate = Matrix.CreateTranslation(dx, dy, 0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            penumbra.BeginDraw();
            Globals.GraphicsDevice.Clear(Color.BlueViolet);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translate);

            map.Drow(spriteBatch);
            player.Drow(spriteBatch);
            //enemy.Drow(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //spriteBatch.DrawString
            //    (font,
            //    $" X: {player.position.X} \n Y: {player.position.Y} \n onGround: {player.onGround} \n {player.CalculateBounds(player.position)}\n {player.velocity}",
            //    new Vector2(50, 50),
            //    Color.White);

            //spriteBatch.DrawString(
            //    font, 
            //    $"{map.UpdatingCpllisions(player.CalculateBounds(player.position + player.velocity)).Count}",
            //    new Vector2(600, 50),
            //    Color.White);

            spriteBatch.End();
        }
    }
}
