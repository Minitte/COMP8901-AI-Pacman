using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TileType[,] tileMatrix;

    public LevelBuilder levelBuilder;

    private void Awake()
    {
        tileMatrix = LevelReader.LoadLevel("level0");

        levelBuilder.BuildLevel(tileMatrix);
    }
}
