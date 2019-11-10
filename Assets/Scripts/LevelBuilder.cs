using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject[] tilePrefabs;

    public void BuildLevel(TileType[,] tileMatrix)
    {
        for (int x = 0; x < tileMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < tileMatrix.GetLength(1); y++)
            {
                GameObject tileGO = Instantiate(tilePrefabs[(int)tileMatrix[x, y]], new Vector2(x, -y), Quaternion.identity);

                tileGO.name = string.Format("{0} [{1},{2}]", tilePrefabs[(int)tileMatrix[x, y]].name, x, y);
            }
        }
    }
}
