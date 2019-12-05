using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DP.Collections;

namespace PathFinding
{
    public static class AStar
    {
        // https://www.redblobgames.com/pathfinding/a-star/introduction.html#astar
        public static List<Vector2Int> FindPath(TileType[,] map, Vector2Int start, Vector2Int end)
        {
            PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> came_from = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, int> cost_so_far = new Dictionary<Vector2Int, int>();

            frontier.Insert(start, 0);
            came_from.Add(start, start);
            cost_so_far.Add(start, 0);

            Vector2Int prev = start;

            while (!frontier.Empty)
            {
                Vector2Int cur = frontier.Pop();

                // arrived
                if (cur == end) break;

                Vector2Int[] neighbours = GetNeighbours(map, cur);
                foreach (Vector2Int neighbour in neighbours)
                {
                    // skip wall
                    if (IsWall(map[neighbour.x, neighbour.y])) continue;

                    int cost = cost_so_far[cur] + 1;

                    // skip if new cost is greater than it's previous cost
                    if (cost_so_far.ContainsKey(neighbour) && cost >= cost_so_far[neighbour]) continue;

                    // insert/update the cost 
                    if (cost_so_far.ContainsKey(neighbour)) cost_so_far[neighbour] = cost;
                    else cost_so_far.Add(neighbour, cost);

                    // insert into the frontier queue or open queue
                    frontier.Insert(neighbour, -(cost + Heuristic.Manhattan(neighbour, end)));

                    // insert/update the came from map
                    if (came_from.ContainsKey(neighbour)) came_from[neighbour] = cur;
                    else came_from.Add(neighbour, cur);
                }

                prev = cur;
            }

            if (!came_from.ContainsKey(end) || !came_from.ContainsKey(start))
            {
                return null;
            }

            return ConstructPath(came_from, start, end);
        }

        private static bool IsWall(TileType t)
        {
            return t == TileType.WALL;
        }

        private static Vector2Int[] GetNeighbours(TileType[,] map, Vector2Int pos)
        {
            int n = 0;
            Vector2Int[] neighbours = new Vector2Int[4];

            if (pos.x > 0) neighbours[n++] = new Vector2Int(pos.x - 1, pos.y);

            if (pos.y > 0) neighbours[n++] = new Vector2Int(pos.x, pos.y - 1);

            if (pos.x < map.GetLength(0) - 1) neighbours[n++] = new Vector2Int(pos.x + 1, pos.y);

            if (pos.y < map.GetLength(1) - 1) neighbours[n++] = new Vector2Int(pos.x, pos.y + 1);

            System.Array.Resize<Vector2Int>(ref neighbours, n);

            return neighbours;
        }

        private static List<Vector2Int> ConstructPath(Dictionary<Vector2Int, Vector2Int> came_from, Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            path.Add(end);

            Vector2Int cur = end;

            while (cur != start)
            {
                Debug.Assert(came_from.ContainsKey(cur), cur);

                cur = came_from[cur];

                path.Add(cur);
            }

            path.Reverse();

            return path;
        }
    }

    public static class Heuristic
    {
        public static int Manhattan(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }

}