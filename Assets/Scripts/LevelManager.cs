using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Tile[,] tiles;

    public LevelInfo info;

    public LevelBuilder levelBuilder;

    public void BuildAndLoad(string level)
    {
        info = LevelReader.LoadLevel(level);

        tiles = levelBuilder.BuildLevel(info.tiles);
    }

    public Tile GetTile(Vector2Int atLocation)
    {
        return tiles[atLocation.x, atLocation.y];
    }

    public Tile GetTile(Vector2Int atLocation, TileDirection towards)
    {
        return tiles[atLocation.x, atLocation.y].neighbours[(int)towards];
    }
}
