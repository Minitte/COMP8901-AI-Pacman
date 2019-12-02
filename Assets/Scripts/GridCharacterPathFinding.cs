using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacterPathFinding : MonoBehaviour
{
    public delegate void TileDelegate(Vector2Int dest);

    public event TileDelegate OnArrive;

    public LevelInfo levelInfo;

    public Vector2Int destination;

    public bool hasPath { get { return m_path != null; } }

    public bool arrived { get { return m_path != null && m_pathIndex >= m_path.Count; } }

    private GridCharacterMovement m_characterMovement;

    private List<Vector2Int> m_path;

    private int m_pathIndex;

    private void Awake()
    {
        m_characterMovement = GetComponent<GridCharacterMovement>();
        destination = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        m_characterMovement.OnArrive += UpdatePath;
    }

    private void Update()
    {
        Vector2Int a = m_characterMovement.coordinate;
        Vector2Int b = destination;
        Debug.DrawLine(new Vector3(a.x, a.y), new Vector3(b.x, b.y), Color.cyan);
    }

    public void GoTo(Vector2Int dest)
    {
        List<Vector2Int> path = PathFinding.AStar.FindPath(levelInfo.tiles, m_characterMovement.coordinate, dest);

        if (path == null) return;

        m_path = path;
        m_pathIndex = 0;

        // skip the first if already there
        if (m_path[m_pathIndex] == m_characterMovement.coordinate) m_pathIndex++;

        destination = dest;

        if (m_characterMovement.coordinate == destination)
        {
            m_characterMovement.GoTo(dest);
            OnArrive?.Invoke(destination);
        }
        else
        {
            m_characterMovement.GoTo(m_path[m_pathIndex]);
        }
    }

    private void UpdatePath(Tile t)
    {
        if (m_path == null) return;

        m_pathIndex++;

        if (m_pathIndex >= m_path.Count)
        {
            OnArrive?.Invoke(destination);
            return;
        }

        m_characterMovement.GoTo(m_path[m_pathIndex]);
    }
}
