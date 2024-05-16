using Microsoft.Xna.Framework;
using ninja.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ninja.Model.BotFields
{
    public class Route
    {
        private List<Rectangle> listVector;
        private Bot bot;
        private int counter;
        public Rectangle CurrentVector
        {
            get
            {
                return listVector[counter];
            }
        }

        public Route(Bot bot)
        {
            this.bot = bot;
            listVector = GetListVector();
        }

        public void Next()
        {
            if (counter + 2 > listVector.Count)
            {
                counter = 0;
                listVector.Reverse();
            }
            else
            {
                counter++;
            }

        }

        private List<Rectangle> GetListVector()
        {
            var pairsVectorValue = LoaderMap.LoadMap("../../../Data/5_BotRoute.csv");

            var start = pairsVectorValue
                .Where(x => x.Value == 1)
                .Select(x => x.Key)
                .FirstOrDefault();
            var end = pairsVectorValue
                .Where(x => x.Value == 2)
                .Select(x => x.Key)
                .FirstOrDefault();

            var pathfinder = new Pathfinding();
            var path = pathfinder.FindPath(pairsVectorValue, start, end, 0);

            var list = path
                .Select(x => new Rectangle(
                    (int)x.X * Tile.Width,
                    (int)x.Y * Tile.Height,
                    (int)(Tile.Width),
                    (int)(Tile.Height*2)))
                .ToList();


            return list;
        }
    }

    public class Pathfinding
    {
        private static readonly Vector2[] Directions = {
        new Vector2(-1, 0),  // Left
        new Vector2(1, 0),   // Right
        new Vector2(0, -1),  // Down
        new Vector2(0, 1),   // Up
        new Vector2(-1, -1), // Left-Down
        new Vector2(1, -1),  // Right-Down
        new Vector2(-1, 1),  // Left-Up
        new Vector2(1, 1)   // Right-Up
        };

        public List<Vector2> FindPath(Dictionary<Vector2, int> field, Vector2 start, Vector2 end, int specialCellType)
        {
            var visited = new HashSet<Vector2>();
            var path = new List<Vector2>();

            if (DFS(field, start, end, specialCellType, visited, path))
            {
                path.Reverse(); // reverse the path to get from start to end
                return path;
            }

            return new List<Vector2>(); // return empty list if no path found
        }

        private bool DFS(Dictionary<Vector2, int> field, Vector2 current, Vector2 end, int specialCellType, HashSet<Vector2> visited, List<Vector2> path)
        {
            if (!field.ContainsKey(current) || (field[current] != specialCellType && field[current] != 1 && field[current] != 2) || visited.Contains(current))
            {
                return false;
            }

            visited.Add(current);

            if (current == end)
            {
                path.Add(current);
                return true;
            }

            foreach (var direction in Directions)
            {
                var neighbor = current + direction;
                if (DFS(field, neighbor, end, specialCellType, visited, path))
                {
                    path.Add(current);
                    return true;
                }
            }

            return false;
        }
    }
}


