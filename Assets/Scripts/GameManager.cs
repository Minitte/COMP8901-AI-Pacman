using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]

    public GameObject playerPrefab;

    public GameObject ghostPrefab;

    [Header("Components")]

    public LevelManager levelManager;

    [Header("Others")]

    public GameObject playerGO;

    public GhostAI[] ghosts;

    public int score;

    public Text scoreText;

    private void Awake()
    {
        levelManager.BuildAndLoad("level2");

        InitPlayer();

        InitGhost();

        SetupPlayerLearning();

        scoreText.text = score + "pt";

        Bit.OnBitCollected += IncScore;
        Bit.OnBitPowerCollected += IncScore10;
    }

    public void IncScore()
    {
        score += 50;
        scoreText.text = score + "pt";
    }

    public void IncScore10()
    {
        score += 500;
        scoreText.text = score + "pt";
    }

    private void InitPlayer()
    {
        playerGO = Instantiate(playerPrefab, new Vector3(levelManager.info.playerStart.x, levelManager.info.playerStart.y), Quaternion.identity);
        GridCharacterMovement movementComp = playerGO.GetComponent<GridCharacterMovement>();
        movementComp.coordinate = levelManager.info.playerStart;
        movementComp.levelManager = levelManager;
        movementComp.GoTo(movementComp.coordinate);

        //Camera.main.GetComponent<FollowGameObject>().target = playerGO;
    }

    private void InitGhost()
    {
        ghosts = new GhostAI[3];

        for (int i = 0; i < 3; i++)
        {
            GameObject ghostGO = Instantiate(ghostPrefab, new Vector3(levelManager.info.ghostSpawn.x, levelManager.info.ghostSpawn.y), Quaternion.identity);

            GridCharacterMovement movementCompG = ghostGO.GetComponent<GridCharacterMovement>();
            movementCompG.coordinate = levelManager.info.ghostSpawn;
            movementCompG.levelManager = levelManager;
            movementCompG.GoTo(levelManager.info.ghostSpawn);

            GhostAI ghostAIComp = ghostGO.GetComponent<GhostAI>();
            ghostAIComp.player = playerGO;
            ghostAIComp.levelinfo = levelManager.info;

            ghostGO.GetComponent<GridCharacterPathFinding>().levelInfo = levelManager.info;

            ghosts[i] = ghostAIComp;
        }
    }

    private void SetupPlayerLearning()
    {
        PlayerLearner_ANN pl_ann = playerGO.GetComponent<PlayerLearner_ANN>();

        if (pl_ann == null) return;

        pl_ann.ghosts = ghosts;

        pl_ann.info = levelManager.info;

        pl_ann.InitANN();
    }
}
