using Microsoft.Xna.Framework;
using ninja.Model;
using System;
using System.Collections.Generic;

namespace ninja.Controller
{
    public static class BotController
    {
        public static List<Bot> Bots = new();
        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < Bots.Count - 1; i++)
            {
                var bot = Bots[i];

                
                bot.StateMachine.CurrentState.Execute();


                //if (PlayerController.Player.BoundingRectangle.Intersects(bot.BoundingRectangle))
                //{
                //    bot.StateMachine.CurrentState = bot.StateMachine.BotStates["Chase"];
                //    EntryPoint.game.Renderer.bots[i].currentAnimation = EntryPoint.game.Renderer.bots[i].attackAnimation;
                //}


                bot.ApplyPhysics(gameTime);

                //(PlayerController.Player.BoundingRectangle.Intersects(bot.AttackRangeRectangle))
                //{

                //}

                EntryPoint.game.Renderer.bots[i].position = bot.Position;
                UpdateState(bot, i);
                UpdateFaceDirection(bot, i);

                if (bot.IsAttacking && PlayerController.Player.BoundingRectangle.Intersects(bot.AttackRangeRectangle))
                {
                    bot.Attack(PlayerController.Player);
                }
                if (PlayerController.Player.IsAttacking && PlayerController.Player.AttackRangeRectangle.Intersects(bot.BoundingRectangle))
                {
                    PlayerController.Player.Attack(bot);
                }
                bot.ResetMovement();
            }
        }
        private static void UpdateState(Bot bot, int i)
        {
            //if (!bot.IsAlive)
            //{
            //    EntryPoint.game.Renderer.bots[i].currentAnimation = EntryPoint.game.Renderer.bots[i].deadAnimation;
            //    return;
            //}
            //if (bot.FieldOfView.Intersects(PlayerController.Player.BoundingRectangle))
            //{
            //    bot.StateMachine.CurrentState = bot.StateMachine.BotStates["Сhase"];
            //    if (PlayerController.Player.BoundingRectangle.Intersects(bot.AttackRangeRectangle))
            //    {
            //        bot.IsAttacking = true;
            //        EntryPoint.game.Renderer.bots[i].currentAnimation = EntryPoint.game.Renderer.bots[i].attackAnimation;
            //    }
            //    else
            //    {
            //        bot.IsAttacking = false;
            //        EntryPoint.game.Renderer.bots[i].currentAnimation = EntryPoint.game.Renderer.bots[i].idleAnimation;
            //    }
            //}
            //else if (Math.Abs(bot.BoundingRectangle.Center.X - PlayerController.Player.BoundingRectangle.Center.X) > 800)
            //{
            //    bot.StateMachine.CurrentState = bot.StateMachine.BotStates["Patrolling"];
            //    EntryPoint.game.Renderer.bots[i].currentAnimation = EntryPoint.game.Renderer.bots[i].idleAnimation;
            //}
        }
        public static void UpdateFaceDirection(Bot bot, int i)
        {
            if (bot.movement > 0)
                EntryPoint.game.Renderer.bots[i].direction = View.Enums.FaceDirect.FaceDirection.Right;
            if (bot.movement < 0)
                EntryPoint.game.Renderer.bots[i].direction = View.Enums.FaceDirect.FaceDirection.Left;
        }
    }
}
