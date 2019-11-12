using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Tile[,] tiles;

    public LevelBuilder levelBuilder;

    private void Awake()
    {
        TileType[,] tileMatrix = LevelReader.LoadLevel("level0");

        tiles = levelBuilder.BuildLevel(tileMatrix);
    }


    public Tile GetTile(Vector2Int atLocation, TileDirection towards)
    {
        return tiles[atLocation.x, atLocation.y].neighbours[(int)towards];
    }
}
