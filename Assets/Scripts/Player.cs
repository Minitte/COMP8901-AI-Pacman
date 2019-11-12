using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterMovement m_movement;

    private void Awake()
    {
        m_movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) m_movement.TrySetDirection(TileDirection.UP);

        if (Input.GetKey(KeyCode.S)) m_movement.TrySetDirection(TileDirection.DOWN);

        if (Input.GetKey(KeyCode.D)) m_movement.TrySetDirection(TileDirection.RIGHT);

        if (Input.GetKey(KeyCode.A)) m_movement.TrySetDirection(TileDirection.LEFT);
    }
}
