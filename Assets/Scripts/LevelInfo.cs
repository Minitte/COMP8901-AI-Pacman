using UnityEngine;

public class LevelInfo
{
    public int width, height;

    public Vector2Int playerStart;

    public TileType[,] tiles;

    public TileType GetTile(Vector2Int atLocation)
    {
        if (atLocation.x < 0 || atLocation.y < 0 || atLocation.x >= width || atLocation.y >= height) return TileType.NULL;

        return tiles[atLocation.x, atLocation.y];
    }

    public TileType GetTile(Vector2Int atLocation, TileDirection towards)
    {
        Vector2Int newLoc = atLocation + TileDirectionVec2.Get_V2I(towards);

        if (newLoc.x < 0 || newLoc.y < 0 || newLoc.x >= width || newLoc.y >= height) return TileType.NULL;

        return tiles[atLocation.x, atLocation.y];
    }
}
