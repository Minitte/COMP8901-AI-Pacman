using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
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
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_pathFinder.GoTo(player.GetComponent<CharacterMovement>().coordinate);
        }
    }
}
