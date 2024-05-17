using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ninja.Model;
using ninja.View.Renderes;
using System.Collections.Generic;

namespace ninja.Controller.Scenes
{
    public class GameScene : Scene
    {
        private TileMap tileMap;
        private List<Rectangle> collisions;
        private Player player;
        
        public GameScene()
        {
            
        }

        public void Initialize()
        {
            tileMap = new TileMap();
            collisions = tileMap.Collisions;
            player = new Player(Vector2.One, collisions, EntryPoint.game.Renderer.player.idleAnimation.RectPositions);
            PlayerController.Player = player;
            player.Position = new(500, 640);


            foreach (var bot in EntryPoint.game.Renderer.bots)
            {
                var bot2 = new Bot(collisions, bot.runAnimation.RectPositions);
                bot2.Position = new(200, 200);
                BotController.Bots.Add(bot2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerController.GetInput(Keyboard.GetState());
            player.ApplyPhysics(gameTime);
            EntryPoint.game.Renderer.player.position = player.Position;

            if (!PlayerController.Player.IsAlive)
            {
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.deadAnimation;
            }
            if (player.Position.Y > 1500)
            {
                PlayerController.Player.Position = new(500, 600);
            }

            if (PlayerController.Player.BoundingRectangle.Intersects(BotController.Bots[1].BoundingRectangle))
            {
                PlayerController.Player.Position = new(500, 600);
            }

            if (PlayerController.Player.BoundingRectangle.Intersects(new Rectangle(0, 400, 100, 100)))
            {
                player.MaxJumpTime = 0.8f;
                player.JumpControlPower = 0.40f;
                player.JumpLaunchVelocity = -2500f;
            }
;





            EntryPoint.game.Renderer.text.Text = "Click   R    for    respawn";

            BotController.Update(gameTime);
            player.ResetToNext();
        }


        public override void Draw(MonogameRenderer renderer)
        {
            renderer.DrawGameMap();
            renderer.DrawPlayer();
            renderer.DrawUI();
        }
    }
}
