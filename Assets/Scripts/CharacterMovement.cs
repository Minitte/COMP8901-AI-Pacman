using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public LevelManager levelManager;

    public TileDirection currentDirection;

    public float speed;

    public Vector2Int coordinate;

    private void Update()
    {
        coordinate = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        levelManager.GetTile(coordinate).ShowDebugLines();

        HandleMoving();
    }

    public bool TrySetDirection(TileDirection dir)
    {
        Tile nextTile = levelManager.GetTile(coordinate, dir);

        // distance check
        float dist = Vector2.Distance(transform.position, coordinate);

        if (dist > 0.1f) return false;

        if (MoveableTile(nextTile))
        {
            currentDirection = dir;
            return true;
        }

        return false;
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
