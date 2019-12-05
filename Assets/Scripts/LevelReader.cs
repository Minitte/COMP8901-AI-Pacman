using System;
using System.IO;

public static class LevelReader
{
    public static readonly string LEVEL_EXT = ".txt";

    public static readonly string LEVEL_DIR = "assets/";

    public static readonly char HEADER_SPILT_SYMBOL = ' ';

    public static LevelInfo LoadLevel(string leveName)
    {
        LevelInfo info = new LevelInfo();

        StreamReader sr = File.OpenText(LEVEL_DIR + leveName + LEVEL_EXT);

        // Read "header"
        string[] header = sr.ReadLine().Split(HEADER_SPILT_SYMBOL);

        // size
        int width = Convert.ToInt32(header[0]);
        int height = Convert.ToInt32(header[1]);
        info.width = width;
        info.height = height;

        // start location
        int startX = Convert.ToInt32(header[2]);
        int startY = Convert.ToInt32(header[3]);
        info.playerStart = new UnityEngine.Vector2Int(startX, startY);

        // ghost spawn location
        int ghostSpawnX = Convert.ToInt32(header[4]);
        int ghostSpawny = Convert.ToInt32(header[5]);
        info.ghostSpawn = new UnityEngine.Vector2Int(ghostSpawnX, ghostSpawny);

        // goal
        int goalX = Convert.ToInt32(header[6]);
        int goalY = Convert.ToInt32(header[7]);
        info.goal = new UnityEngine.Vector2Int(goalX, goalY);

        TileType[,] tileMatrix = new TileType[width, height];

        // read level
        for (int y = 0; y < height; y++)
        {
            string row = sr.ReadLine();

            for (int x = 0; x < width; x++)
            {
                TileType type = (TileType)(int)Char.GetNumericValue(row[x]);

                tileMatrix[x, y] = type;
            }
        }

        info.tiles = tileMatrix;

        return info;
    }
}
