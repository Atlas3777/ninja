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

            var pathfinder = new PathFinding();
            var path = pathfinder.FindPathDFS(pairsVectorValue, start, end, 0);

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

    public class PathFinding
    {
        private static readonly Vector2[] Directionss = {
        new Vector2(-1, 0),  
        new Vector2(1, 0),   
        new Vector2(0, -1), 
        new Vector2(0, 1),   
        new Vector2(-1, -1), 
        new Vector2(1, -1),
        new Vector2(-1, 1),  
        new Vector2(1, 1)   
        };

        public List<Vector2> FindPathDFS(Dictionary<Vector2, int> field, Vector2 start, Vector2 end, int specialCellType)
        {
            var visited = new HashSet<Vector2>();
            var path = new List<Vector2>();

            if (DFS(field, start, end, specialCellType, visited, path))
            {
                path.Reverse();
                return path;
            }

            return new List<Vector2>(); 
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

            foreach (var direction in Directionss)
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

        private static readonly Vector2[] Directions = {
            new Vector2(-1, 0),
            new Vector2(1, 0),   
            new Vector2(0, -1),  
        };

        public List<Vector2> FindPathBFS2(Dictionary<Vector2, int> field, Vector2 start, Vector2 end)
        {
            if (!field.ContainsKey(start) || !field.ContainsKey(end))
            {
                return new List<Vector2>();
            }

            var queue = new Queue<Vector2>();
            var visited = new HashSet<Vector2>();
            var parent = new Dictionary<Vector2, Vector2>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == end)
                {
                    var path = new List<Vector2>();
                    while (current != start)
                    {
                        path.Add(current);
                        current = parent[current];
                    }
                    path.Add(start);
                    path.Reverse(); 
                    return path;
                }

                foreach (var direction in Directions)
                {
                    var neighbor = current + direction;
                    if (field.ContainsKey(neighbor) && field[neighbor] == -1 && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                    }
                }
            }

            return new List<Vector2>();
        }

    }
}


