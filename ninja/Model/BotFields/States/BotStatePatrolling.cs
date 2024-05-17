using ninja.Controller;
using ninja.Extensions;
using System;

namespace ninja.Model.BotFields.States
{
    public class BotStatePatrolling : BotState
    {
        public Bot bot;
        public BotStatePatrolling(Bot bot)
        {
            this.bot = bot;
        }

        public override void Execute()
        {
            MoveToNextPoint(300f);
        }
        public void MoveToNextPoint(float speed)
        {
            if (bot.BoundingRectangle.GetIntersectionDepth(bot.Route.CurrentVector) != Microsoft.Xna.Framework.Vector2.Zero)
            {
                bot.Route.Next();
            }
            var newPoint = bot.Route.CurrentVector;

            if (bot.BoundingRectangle.Bottom > newPoint.Bottom)
            {
                bot.isJumping = true;
            }

            var rnd = new Random();
            if (rnd.Next(1, 20) == 1)
            {
                bot.isJumping = false;
            }

            if (bot.BoundingRectangle.Center.X < newPoint.Center.X)
                bot.movement = speed;
            else 
                bot.movement = -speed;

        }
    }
}
