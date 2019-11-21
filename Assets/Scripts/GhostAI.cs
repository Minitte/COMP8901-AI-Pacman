using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    enum GhostState
    { 
        WANDER,
        CHASE
    }

    public LevelInfo levelinfo;

    public GameObject player;

    public float chaseDistance;

    private CharacterMovement m_characterMovement;

    private CharacterPathfinding m_pathFinder;

    private GhostState m_state;

    private float m_stateUpdate;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_pathFinder = GetComponent<CharacterPathfinding>();
    }

    private void Update()
    {
        m_stateUpdate += Time.deltaTime;

        if (m_stateUpdate < 0.5f) return;

        m_stateUpdate = 0;

        if (Vector2.Distance(player.transform.position, transform.position) < chaseDistance)
        {
            m_state = GhostState.CHASE;
        }
        else
        {
            m_state = GhostState.WANDER;
        }

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
        m_pathFinder.GoTo(levelinfo.GetRandomOpenTile());
    }

    private void HandleChase()
    {
        m_pathFinder.GoTo(player.GetComponent<CharacterMovement>().coordinate);
    }
}
