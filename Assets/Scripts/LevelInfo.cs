using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    public int width, height;

    public Vector2Int playerStart;

    public Vector2Int ghostSpawn;

    public Vector2Int goal;

    public TileType[,] tiles;

    private List<Vector2Int> m_openTiles;

    public Vector2Int GetRandomOpenTile()
    {
        if (m_openTiles == null) IniOpenTiles();

        return m_openTiles[Random.Range(0, m_openTiles.Count)];
    }

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

    private void IniOpenTiles()
    {
        m_openTiles = new List<Vector2Int>();

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                if (tiles[x, y] == TileType.OPEN) m_openTiles.Add(new Vector2Int(x, y));
            }
        }
    }
}
