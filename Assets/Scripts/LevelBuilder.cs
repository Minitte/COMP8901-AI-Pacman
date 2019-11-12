using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject[] tilePrefabs;

    public Tile[,] BuildLevel(TileType[,] tileMatrix)
    {
        Tile[,] tiles = CreateTiles(tileMatrix);

        AssignNeighbours(tiles);

        return tiles;
    }

    /// <summary>
    /// Instantiate all of the tile game objects.
    /// </summary>
    private Tile[,] CreateTiles(TileType[,] tileMatrix)
    {
        Tile[,] tiles = new Tile[tileMatrix.GetLength(0), tileMatrix.GetLength(1)];

        for (int x = 0; x < tileMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < tileMatrix.GetLength(1); y++)
            {
                GameObject tileGO = Instantiate(tilePrefabs[(int)tileMatrix[x, y]], new Vector2(x, y), Quaternion.identity);

                Tile tileComp = tileGO.GetComponent<Tile>();
                tiles[x, y] = tileComp;

                tileComp.coordinate = new Vector2(x, y);
                tileComp.tileType = tileMatrix[x, y];

                tileGO.name = string.Format("{0} [{1},{2}]", tilePrefabs[(int)tileMatrix[x, y]].name, x, y);
            }
        }

        return tiles;
    }

    /// <summary>
    /// Assign neighbours for all tiles.
    /// </summary>
    private void AssignNeighbours(Tile[,] tiles)
    {
        int xMax = tiles.GetLength(0);
        int yMax = tiles.GetLength(1);

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                Tile cur = tiles[x, y];

                // up
                if (y + 1 < yMax) cur.SetNeighbour(TileDirection.UP, tiles[x, y + 1]); 

                // right
                if (x + 1 < xMax) cur.SetNeighbour(TileDirection.RIGHT, tiles[x + 1, y]);

                // down
                if (y - 1 >= 0) cur.SetNeighbour(TileDirection.DOWN, tiles[x, y - 1]);

                // left
                if (x - 1 >= 0) cur.SetNeighbour(TileDirection.LEFT, tiles[x - 1, y]);
            }
        }
    }

}
