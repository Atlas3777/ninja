using System;
using Microsoft.Xna.Framework;
using ninja.Model;

namespace ninja.Model.BotFields.States
{
    public class BotStateСhase : BotState
    {
        public Bot bot;
        public Player player;
        public BotStateСhase(Bot bot)
        {
            this.bot = bot;
        }

        public override void Execute()
        {



            RR(400);


        }
        private void RR(float speed)
        {
            if (Math.Abs(bot.Route.CurrentVector.Center.X - bot.BoundingRectangle.Center.X) <= 16 && bot.BoundingRectangle.Bottom <= bot.Route.CurrentVector.Bottom)
            {
                bot.Route.Next();
            }
            var newPoint = bot.Route.CurrentVector;

            if (bot.BoundingRectangle.Bottom > newPoint.Bottom)
            {
                bot.isJumping = true;
            }

            if (bot.BoundingRectangle.Center.X < newPoint.Center.X)
                bot.movement += speed;
            else
                bot.movement += -speed;
        }
    }
}
