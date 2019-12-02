using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacterMovement : MonoBehaviour
{
    public delegate void TileDelegate(Tile t);

    // when this character moves on a new tile
    public event TileDelegate OnArrive;

    public LevelManager levelManager;

    public float speed;

    public Vector2Int coordinate;

    public bool isMoving { get { return m_lerpFactor < 1; } }

    private Vector2Int m_targetCoord;

    private float m_lerpFactor;

    private void Update()
    {
        if (!isMoving) return;

        m_lerpFactor += Time.deltaTime * speed;

        transform.position = Vector2.Lerp(coordinate, m_targetCoord, m_lerpFactor);

        // arrived
        if (!isMoving)
        {
            coordinate = m_targetCoord;
            OnArrive?.Invoke(levelManager.GetTile(coordinate));
        }
    }

    public void GoTo(Vector2Int target)
    {
        if (coordinate == target)
        {
            OnArrive?.Invoke(levelManager.GetTile(coordinate));
            return;
        }

        m_targetCoord = target;
        m_lerpFactor = 0;
    }
}
