using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum MovementState
    {
        STOPPED,
        MOVING
    }

    public LevelManager levelManager;

    public TileDirection currentDirection;

    public float speed;

    public Vector2Int coordinate;

    private MovementState m_state;

    private void Update()
    {
        coordinate = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        HandleMoving();
    }

    public Tile TryGetTile(TileDirection dir)
    {
        Tile t = levelManager.GetTile(coordinate, currentDirection);

        if (!MoveableTile(t)) return null;

        return t;
    }

    public bool MoveableTile(Tile t)
    {
        return t != null && t.tileType != TileType.WALL;
    }

    private void HandleMoving()
    {
        Vector2 dir = TileDirectionVec2.Get_V2F(currentDirection);

        transform.position = (Vector2)transform.position + (dir * speed * Time.deltaTime);

        // check if we can go further

        Tile nextTile = levelManager.GetTile(coordinate, currentDirection);

        if (!MoveableTile(nextTile))
        {
            float distToNexTile = Vector2.Distance(transform.position, nextTile.coordinate);

            if (distToNexTile < 1f)
            {
                transform.position = new Vector3(coordinate.x, coordinate.y);
            }
        }
    }
}
