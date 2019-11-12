using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGhostController : MonoBehaviour
{
    private CharacterMovement m_movement;

    private bool m_changeDir;

    private void Awake()
    {
        m_movement = GetComponent<CharacterMovement>();

        m_movement.OnBlocked += RandomUnbock;
    }

    private void RandomUnbock(Tile t)
    {
        TileDirection tryDIr = (TileDirection) Random.Range(0, 4);

        while (true)
        {
            if (m_movement.TrySetDirection(tryDIr)) break;

            tryDIr = (TileDirection)(((int)tryDIr + 1) % 4);
        }
    }
}
