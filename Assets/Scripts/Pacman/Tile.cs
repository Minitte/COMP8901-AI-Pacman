using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coordinate;

    public TileType tileType;

    public Tile[] neighbours = new Tile[4];

    public int numNeighbours
    {
        get {
            int n = 0;
            if (neighbours[0] != null) n++;
            if (neighbours[1] != null) n++;
            if (neighbours[2] != null) n++;
            if (neighbours[3] != null) n++;
            return n;
        }
    }

    public void SetNeighbour(TileDirection dir, Tile t)
    {
        neighbours[(int)dir] = t;
    }

    public void ShowDebugLines()
    {
        Vector3 pos1 = transform.position;

        foreach (Tile t in neighbours)
        {
            if (t == null) continue;

            Vector3 pos2 = t.transform.position;
            if (t.tileType != TileType.WALL) Debug.DrawLine(pos1, pos2, Color.white);
            else Debug.DrawLine(pos1, pos2, Color.red);


        }
    }

}
