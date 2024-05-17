using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ninja.Controller;
using ninja.Extensions;
using ninja.Model.BotFields.States;

namespace ninja.Model.BotFields.States
{
    public class BotStateСhase : BotState
    {
        private Bot bot;
        private float posInTileX;
        private float posInTileY;
        private float posPlayerInTileX;
        private float posPlayerInTileY;
        public BotStateСhase(Bot bot)
        {
            this.bot = bot;
        }

        public override void Execute()
        {
            var pathToPlayer = FindPathToPlayer();

            if (pathToPlayer != null && pathToPlayer.Count > 0)
                MoveToNextPoint(pathToPlayer);

        }

        private List<Rectangle> FindPathToPlayer()
        {
             posInTileX = (float)Math.Floor((float)bot.BoundingRectangle.Center.X / Tile.Width);
             posInTileY = (float)Math.Floor((float)bot.BoundingRectangle.Bottom / Tile.Height)-1;

             posPlayerInTileX = (float)Math.Floor((float)PlayerController.Player.BoundingRectangle.Center.X / Tile.Width);
             posPlayerInTileY = (float)Math.Floor((float)PlayerController.Player.BoundingRectangle.Bottom / Tile.Height)-1;


            var botPosition = new Vector2(posInTileX, posInTileY);
            var playerPosition = new Vector2(posPlayerInTileX, posPlayerInTileY);

            var pathfinder = new PathFinding();


            var map = LoaderMap.LoadMap1("../../../Data/5_Collisions.csv");

            List<Vector2> path = pathfinder.FindPathBFS2(map, botPosition, playerPosition);

            var list = path
                .Select(x => new Rectangle(
                    (int)x.X * Tile.Width,
                    (int)x.Y * Tile.Height,
                    (int)(Tile.Width),
                    (int)(Tile.Height)))
                .ToList();

            return list;
        }

        public void MoveToNextPoint(List<Rectangle> path)
        {
            if (path == null)
                return;

            if (bot.BoundingRectangle.GetIntersectionDepth(path.First()) != Vector2.Zero);
            {
                path.RemoveAt(0);
            }

            if (path.Count==0)
                return;

            var newPoint = path.First();
            var botRect = bot.BoundingRectangle;

            if (botRect.Bottom > newPoint.Bottom)
                bot.isJumping = true;

            if (botRect.Center.X > newPoint.Center.X)
                bot.movement = -300;
            else
                bot.movement = 300;

            var rnd = new Random();
            if (rnd.Next(1, 20) == 1)
            {
                bot.isJumping = false;
            }
        }
    }
}
