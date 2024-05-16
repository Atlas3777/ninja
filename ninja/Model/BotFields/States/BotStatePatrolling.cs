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
            if (bot.BoundingRectangle.Intersects(bot.Route.CurrentVector))
            {
                bot.Route.Next();
            }
            var newPoint = bot.Route.CurrentVector;

            if (bot.BoundingRectangle.Bottom > newPoint.Bottom)
            {
                bot.isJumping = true;
            }

            if (bot.BoundingRectangle.Center.X < newPoint.Center.X)
                bot.movement = speed;
            else 
                bot.movement = -speed;


        }
    }
}
