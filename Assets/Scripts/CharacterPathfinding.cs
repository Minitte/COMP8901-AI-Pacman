using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPathfinding : MonoBehaviour
{
    public LevelInfo levelInfo;
    
    private CharacterMovement m_characterMovement;

    private List<Vector2Int> m_path;

    private int m_pathIndex;

    private TileDirection m_nextDir;

    private void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_characterMovement.OnNewTile += UpdatePath;
    }

    private void Update()
    {
        if (m_path == null) return;

        if (m_pathIndex >= m_path.Count) return;

        m_characterMovement.TrySetDirection(m_nextDir);
    }

    public void GoTo(Vector2Int dest)
    {
        m_path = PathFinding.AStar.FindPath(levelInfo.tiles, m_characterMovement.coordinate, dest);
        m_pathIndex = 0;

        // skip the first if already there
        if (m_path[m_pathIndex] == m_characterMovement.coordinate) m_pathIndex++;

        m_nextDir = TileDirectionEnum.Get_TD(m_characterMovement.coordinate, m_path[m_pathIndex]);
    }

    private void UpdatePath(Tile t)
    {
        if (m_path == null) return;

        m_pathIndex++;

        if (m_pathIndex >= m_path.Count) return;

        m_nextDir = TileDirectionEnum.Get_TD(m_characterMovement.coordinate, m_path[m_pathIndex]);
    }

}
