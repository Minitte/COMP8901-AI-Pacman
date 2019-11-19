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

        for (int i = 0; i < 4; i++)
        {
            GameObject ghostGO = Instantiate(ghostPrefab, new Vector3(levelManager.info.ghostSpawn.x, levelManager.info.ghostSpawn.y), Quaternion.identity);
            CharacterMovement movementCompG = ghostGO.GetComponent<CharacterMovement>();
            movementCompG.coordinate = levelManager.info.ghostSpawn;
            movementCompG.levelManager = levelManager;
        }
    }
}
