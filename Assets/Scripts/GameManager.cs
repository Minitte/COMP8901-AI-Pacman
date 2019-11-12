using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]

    public GameObject playerPrefab;

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
    }
}
