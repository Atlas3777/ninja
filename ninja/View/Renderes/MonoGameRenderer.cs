using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ninja.Controller;
using ninja.View.ObjectRender;
using System.Collections.Generic;

namespace ninja.View.Renderes
{
    public class MonogameRenderer : AbstactRenderer
    {
        private ContentManager Content = EntryPoint.game.Content;
        private GraphicsDevice GraphicsDevice = EntryPoint.game.GraphicsDevice;
        private SpriteBatch SpriteBatch;

        private Background background;
        private Background background1;
        public PlayerV player;
        public TileMapV tileMap;
        public TextV text;

        private Matrix translate;
        private Matrix trans;

        private Background mainMenuBackground;
        public Button buttonStart;
        public Button buttonExit;

        public BotV bot;
        public List<BotV> bots;

        public MonogameRenderer()
        {
            this.player = ORInitializer.CreatePlayer(Content);
            this.background = ORInitializer.CreateBackground(Content);
            this.background1 = ORInitializer.CreateBackground1(Content);
            this.mainMenuBackground = ORInitializer.CreateBackgroundMainMenu(Content);
            this.buttonStart = ORInitializer.CreateStartButton(Content);
            this.buttonExit = ORInitializer.CreateExitButton(Content);
            this.tileMap = ORInitializer.CreateTileMapV(Content);
            this.text = ORInitializer.CreateText(Content);
            this.bot = ORInitializer.CreateBot(Content);
            this.bots = ORInitializer.CreateListBots(Content);
        }

        public override void DrawPlayer()
        {
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translate);
            player.UpdateAnimation();
            player.Draw(SpriteBatch);
            SpriteBatch.End();
        }

        public override void DrawGameMap()
        {
            CalculateTranslate();
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            background.Drow(SpriteBatch);
            SpriteBatch.End();


            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: trans);
            background1.Drow(SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translate);
            tileMap.Draw(SpriteBatch);
            foreach (var bot in bots)
            {
                bot.Draw(SpriteBatch);
            }
            //bot.Draw(SpriteBatch);
            SpriteBatch.End();
        }


        public override void DrawUI()
        {
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            text.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void DrawMenu()
        {
            this.SpriteBatch = EntryPoint.game.SpriteBatch;
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mainMenuBackground.Drow(SpriteBatch);
            buttonStart.Draw(SpriteBatch);
            buttonExit.Draw(SpriteBatch);
            SpriteBatch.End();

        }
        private void CalculateTranslate()
        {

            var dx = 1920 / 2 - player.position.X - player.idleAnimation.RectPositions.Width / 2;
            dx = MathHelper.Clamp(dx, -120 * 32 - 1920, 0);
            //var dy = 1080 / 2 - player.position.Y - player.idleAnimation.RectPositions.Height / 2;
            //dy = MathHelper.Clamp(dy, -100, 0);
            translate = Matrix.CreateTranslation(dx, 0, 0);
            var dx1 = dx *= 0.1f;
            trans = Matrix.CreateTranslation(dx1, 0, 0);

        }
    }
}
