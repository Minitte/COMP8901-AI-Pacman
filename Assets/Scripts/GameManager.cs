using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]

    public GameObject playerPrefab;

    public GameObject ghostPrefab;

    [Header("Components")]

    public LevelManager levelManager;

    private void Awake()
    {
        levelManager.BuildAndLoad("level0");

        GameObject playerGO = Instantiate(playerPrefab, new Vector3(levelManager.info.playerStart.x, levelManager.info.playerStart.y), Quaternion.identity);
        CharacterMovement movementComp = playerGO.GetComponent<CharacterMovement>();
        movementComp.coordinate = levelManager.info.playerStart;
        movementComp.levelManager = levelManager;

        Camera.main.GetComponent<FollowGameObject>().target = playerGO;

        for (int i = 0; i < 1; i++)
        {
            GameObject ghostGO = Instantiate(ghostPrefab, new Vector3(levelManager.info.ghostSpawn.x, levelManager.info.ghostSpawn.y), Quaternion.identity);

            GridCharacterMovement movementCompG = ghostGO.GetComponent<GridCharacterMovement>();
            movementCompG.coordinate = levelManager.info.ghostSpawn;
            movementCompG.levelManager = levelManager;
            movementCompG.GoTo(levelManager.info.ghostSpawn);

            ghostGO.GetComponent<GhostAI>().player = playerGO;
            ghostGO.GetComponent<GhostAI>().levelinfo = levelManager.info;

            ghostGO.GetComponent<GridCharacterPathFinding>().levelInfo = levelManager.info;
        }
    }
}
