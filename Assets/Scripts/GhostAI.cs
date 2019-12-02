using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public LevelInfo levelinfo;

    public GameObject player;

    public GridCharacterMovement characterMovement;

    public GridCharacterPathFinding pathFinder;

    private void Awake()
    {
        characterMovement = GetComponent<GridCharacterMovement>();
        pathFinder = GetComponent<GridCharacterPathFinding>();
    }

    private void Start()
    {
        pathFinder.GoTo(levelinfo.GetRandomOpenTile());
    }
    
}
