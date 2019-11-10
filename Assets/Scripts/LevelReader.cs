using System;
using System.IO;

public static class LevelReader
{
    public static readonly string LEVEL_EXT = ".txt";

    public static readonly string LEVEL_DIR = "assets/";

    public static readonly char HEADER_SPILT_SYMBOL = ' ';

    public static TileType[,] LoadLevel(string leveName)
    {
        StreamReader sr = File.OpenText(LEVEL_DIR + leveName + LEVEL_EXT);

        // Read "header"
        string[] header = sr.ReadLine().Split(HEADER_SPILT_SYMBOL);

        int width = Convert.ToInt32(header[0]);
        int height = Convert.ToInt32(header[1]);

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

        return tileMatrix;
    }
}
