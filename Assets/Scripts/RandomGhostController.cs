using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGhostController : MonoBehaviour
{
    private CharacterMovement m_movement;

    private bool m_changeWhenCenter;

    private void Awake()
    {
        m_movement = GetComponent<CharacterMovement>();

        m_movement.OnBlocked += RandomUnbock;
        m_movement.OnNewTile += RandomDirChange;
    }

    private void Update()
    {
        if (m_changeWhenCenter && m_movement.IsCloseToCenter())
        {
            m_changeWhenCenter = false;
            ChangeDir();
        }
    }   

    private void RandomDirChange(Tile t)
    {
        int roll = Random.Range(0, 100);

        switch (t.numNeighbours)
        {
            case 2:
                if (roll < 15) m_changeWhenCenter = true;
                break;
            case 3:
                if (roll < 30) m_changeWhenCenter = true;
                break;
            case 4:
                if (roll < 20) m_changeWhenCenter = true;
                break;
        }
    }

    private void RandomUnbock(Tile t)
    {
        ChangeDir();
    }

    private void ChangeDir()
    {
        TileDirection tryDIr = (TileDirection)(((int)m_movement.currentDirection + Random.Range(1, 4)) % 4);

        while (true)
        {
            if (m_movement.TrySetDirection(tryDIr)) break;

            tryDIr = (TileDirection)(((int)tryDIr + 1) % 4);
        }
    }
}
