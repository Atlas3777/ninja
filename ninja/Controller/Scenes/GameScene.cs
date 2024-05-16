using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ninja.Model;
using ninja.View.Renderes;
using System.Collections.Generic;

namespace ninja.Controller.Scenes
{
    public class GameScene : Scene
    {
        TileMap tileMap;
        List<Rectangle> collisions;
        Player player;
        Bot bot;
        
        public GameScene()
        {
            tileMap = new TileMap();
            collisions = tileMap.collisions;
            var playerPos = EntryPoint.game.Renderer.player.position;

            //bot = new Bot(collisions, EntryPoint.game.Renderer.bot.Sprite.Rectangle);

            player = new Player(playerPos, collisions, EntryPoint.game.Renderer.player.idleAnimation.RectPositions, 0, 0);

            player.Position = new(2000, 600);
            var i = 1;
            foreach (var bot in EntryPoint.game.Renderer.bots)
            {
                bot.Sprite.Rectangle.Location = new Point(i * 700, bot.Sprite.Rectangle.Y);
                var bot2 = new Bot(collisions, bot.Sprite.Rectangle);
                bot2.Position = new(2000, 600);
                BotController.bots.Add(bot2);
                i++;
            }
            //BotController.bots.Add(bot);
        }
        


        public override void Update(GameTime gameTime)
        {
            
            var keyboard = Keyboard.GetState();


            if (keyboard.IsKeyDown(Keys.D))
            {
                player.movement = 300f;
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.runAnimation;
                EntryPoint.game.Renderer.player.direction = View.ObjectRender.PlayerV.FaceDirection.Right;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                player.movement = -300f;
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.runAnimation;
                EntryPoint.game.Renderer.player.direction = View.ObjectRender.PlayerV.FaceDirection.Left;
            }
            else
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.idleAnimation;

            if (keyboard.IsKeyDown(Keys.Space))
            {
                player.isJumping = true;
                
            }
            //if(player.isJumping)
            //    EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.jumpAnimation;


            player.ApplyPhysics(gameTime);

            BotController.Update(gameTime, player.BoundingRectangle);
           
            //var text = $"{player.Velocity}  \n{player.movement}\n{player.IsOnGround}\n{player.Position}";


            EntryPoint.game.Renderer.player.position = player.Position;
            //EntryPoint.game.Renderer.text.Text = text;

            player.movement = 0.0f;
            player.isJumping = false;
        }

        public override void Draw(MonogameRenderer renderer)
        {
            renderer.DrawGameMap();
            renderer.DrawPlayer();
            renderer.DrawUI();
        }
    }
}
