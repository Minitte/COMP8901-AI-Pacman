using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI_FSM : MonoBehaviour
{
    enum GhostState
    {
        WANDER,
        CHASE
    }

    public float chaseDistance;

    private GhostAI ghost;

    private GridCharacterMovement m_characterMovement { get { return ghost.characterMovement; } }

    private GridCharacterPathFinding m_pathFinder { get { return ghost.pathFinder; } }

    private LevelInfo m_levelInfo { get { return ghost.levelinfo; } }

    private GameObject m_player { get { return ghost.player; } }

    private GhostState m_state;

    private float m_stateUpdate;

    private float m_delay = 0.25f;

    private bool m_aquiredPlayer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private void Awake()
    {
        ghost = GetComponent<GhostAI>();

        m_pathFinder.OnArrive += HandleOnArrive;
    }

    private void Update()
    {
        m_stateUpdate += Time.deltaTime;

        if (Vector2.Distance(m_player.transform.position, transform.position) < chaseDistance)
        {
            m_state = GhostState.CHASE;
            m_delay = 0.1f;
        }
        else
        {
            m_state = GhostState.WANDER;
            m_delay = 5f;
        }

        if (m_stateUpdate < m_delay) return;

        m_stateUpdate = 0;

        switch (m_state)
        {
            case GhostState.WANDER:
                HandleWander();
                break;

            case GhostState.CHASE:
                HandleChase();
                break;
        }
    }

    private void HandleWander()
    {
        m_aquiredPlayer = false;
        m_pathFinder.GoTo(m_levelInfo.GetRandomOpenTile());
    }

    private void HandleChase()
    {
        if (m_pathFinder.arrived || !m_aquiredPlayer)
        {
            m_aquiredPlayer = true;
            m_pathFinder.GoTo(m_player.GetComponent<CharacterMovement>().coordinate);
        }
    }

    private void HandleOnArrive(Vector2Int t)
    {
        if (m_state == GhostState.WANDER)
        {
            m_aquiredPlayer = false;
            m_pathFinder.GoTo(m_levelInfo.GetRandomOpenTile());
            m_stateUpdate = 0;
        }
        else if (m_state == GhostState.WANDER)
        {
            m_aquiredPlayer = true;
            m_pathFinder.GoTo(m_player.GetComponent<CharacterMovement>().coordinate);
        }
    }
}
