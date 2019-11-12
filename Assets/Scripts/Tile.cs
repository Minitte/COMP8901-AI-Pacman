using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coordinate;

    public TileType tileType;

    public Tile[] neighbours = new Tile[4];

    public void SetNeighbour(TileDirection dir, Tile t)
    {
        neighbours[(int)dir] = t;
    }

    private void OnMouseOver()
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
