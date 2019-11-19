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

    private CharacterMovement m_characterMovement;

    private CharacterPathfinding m_pathFinder;

    private void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_pathFinder = GetComponent<CharacterPathfinding>();
    }

    private void Update()
    {
        
    }
}
