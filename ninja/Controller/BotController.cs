using Microsoft.Xna.Framework;
using ninja.Model;
using System;
using System.Collections.Generic;

namespace ninja.Controller
{
    public static class BotController
    {
        public static List<Bot> bots = new();
        public static void Update(GameTime gameTime, Rectangle playerPos)
        {
            for(int i = 0; i < bots.Count-1; i++)
            {
                Bot bot = bots[i];
                

                //bot.stateMachine.CurrentState = bot.stateMachine.BotStates["Patrolling"];

                bot.stateMachine.CurrentState.Execute();

                bot.ApplyPhysics(gameTime);

                var text = $"{bot.Velocity} \n{bot.movement}\n{bot.IsOnGround}\n{bot.Position.X}\n{bot.Position.Y}\n{bot.isJumping}\n{bot.Route.CurrentVector}";

                EntryPoint.game.Renderer.text.Text = text;
                EntryPoint.game.Renderer.bots[i].Sprite.Rectangle.Location = bot.Position.ToPoint();
                bot.movement = 0;
                bot.isJumping = false;
            }
        }
        private static void UpdateState()
        {



        }
    }
}
