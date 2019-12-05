using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLearner_ANN : MonoBehaviour
{
    [Header("Info for ANN")]

    public Player player;

    public GhostAI[] ghosts;

    public LevelInfo info;

    private ANN.ArtificalNerualNetwork m_ann;

    private TileDirection lastPrediction;

    private int trainCounter;

    private void Awake()
    {
        player = GetComponent<Player>(); 
    }

    public void InitANN()
    {
        if (m_ann == null) 
        {
            m_ann = new ANN.ArtificalNerualNetwork(new int[] { 13, 8, 8, 4 });
        }
    }

    public void PredictAndTrain(TileDirection expectedDirection)
    {
        float[] input = new float[]
        {
            // ghost info (x6)
            ghosts[0].coordinate.x, // ghost 1
            ghosts[0].coordinate.y, // ghost 1
            ghosts[1].coordinate.x, // ghost 2
            ghosts[1].coordinate.y, // ghost 2
            ghosts[2].coordinate.x, // ghost 3
            ghosts[2].coordinate.y, // ghost 3

            // player info (x2)
            player.coordinate.x,
            player.coordinate.y,

            // level info (x5)
            IsWallAtDirection(TileDirection.UP),
            IsWallAtDirection(TileDirection.RIGHT),
            IsWallAtDirection(TileDirection.DOWN),
            IsWallAtDirection(TileDirection.LEFT),

            PathFinding.Heuristic.Manhattan(player.coordinate, info.ghostSpawn)
        };

        float[] expected = new float[]
        {
            expectedDirection == TileDirection.UP ? 1f : 0f,
            expectedDirection == TileDirection.RIGHT ? 1f : 0f,
            expectedDirection == TileDirection.DOWN ? 1f : 0f,
            expectedDirection == TileDirection.LEFT ? 1f : 0f
        };

        // try to predict first.
        float[] actual = m_ann.Forward(input);

        // find best
        TileDirection bestDirection = TileDirection.UP;
        float bestValue = actual[0];
        for (int i = 1; i < actual.Length; i++)
        {
            if (actual[i] > bestValue)
            {
                bestDirection = (TileDirection)i;
                bestValue = actual[i];
            }
        }

        lastPrediction = bestDirection;

        m_ann.Train(input, expected);
    }

    /// <summary>
    /// Checks if the tile in a direction from the player is a wall
    /// </summary>
    /// <param name="dir">Direction from player</param>
    /// <returns>1 if not wall. 0 if wall</returns>
    private float IsWallAtDirection(TileDirection dir)
    {
        return info.GetTile(player.coordinate + TileDirectionVec2.Get_V2I(dir)) != TileType.WALL ? 1 : 0;
    }
}
