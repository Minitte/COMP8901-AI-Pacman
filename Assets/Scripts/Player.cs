using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2Int coordinate { get { return m_movement.coordinate; } }

    private GridCharacterMovement m_movement;

    private TileDirection m_inputDir;

    private void Awake()
    {
        m_movement = GetComponent<GridCharacterMovement>();
    }

    private void LateUpdate()
    {

        if (Input.GetKey(KeyCode.W)) m_inputDir = TileDirection.UP;
        else if (Input.GetKey(KeyCode.D)) m_inputDir = TileDirection.RIGHT;
        else if (Input.GetKey(KeyCode.S)) m_inputDir = TileDirection.DOWN;
        else if (Input.GetKey(KeyCode.A)) m_inputDir = TileDirection.LEFT;
        else m_inputDir = TileDirection.NONE;

        if (!m_movement.isMoving && m_inputDir != TileDirection.NONE)
        {
            m_movement.GoTo(coordinate + TileDirectionVec2.Get_V2I(m_inputDir));
        }
    }
}
