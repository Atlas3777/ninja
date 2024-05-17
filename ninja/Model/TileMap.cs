using Microsoft.Xna.Framework;
using ninja.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace ninja.Model
{
    public class TileMap
    {
        public List<Rectangle> Collisions = new();
        public TileMap()
        {
            var tileMap = LoaderMap.LoadMap("../../../Data/5_Collisions.csv");

            foreach (var tile in tileMap)
            {
                Collisions.Add(new Rectangle(
                (int)tile.Key.X * 32,
                (int)tile.Key.Y * 32,
                32, 32));
            }
        }
    }
}
