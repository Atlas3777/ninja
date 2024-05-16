using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ninja.Model
{
    public class TileMap
    {
        public List<Rectangle> collisions = new();
        public TileMap()
        {
            var tileMap = LoadMap("../../../Data/5_Collisions.csv");

            foreach (var tile in tileMap)
            {
                    collisions.Add(new Rectangle(
                    (int)tile.Key.X * 32,
                    (int)tile.Key.Y * 32,
                    32, 32));
            }
        }

            public static Dictionary<Vector2, int> LoadMap(string filepath)
            {
                var result = new Dictionary<Vector2, int>();

                var reader = new StreamReader(filepath);

                int y = 0;
                int xSize = 0;
                string line;


                while ((line = reader.ReadLine()) != null)
                {
                    var items = line.Split(',');
                    xSize = Math.Max(items.Length, xSize);

                    for (int x = 0; x < items.Length; x++)
                    {
                        if (int.TryParse(items[x], out int value))
                        {
                            if (value > -1)
                            {
                                result[new Vector2(x, y)] = value;
                            }
                        }
                    }
                    y++;
                }

                return result;
            }
    }
}
