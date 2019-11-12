using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public delegate void TileDelegate(Tile t);

    // when this character moves on a new tile
    public event TileDelegate OnNewTile;

    // When this character can't move because it's blocked by a wall
    public event TileDelegate OnBlocked;

    public LevelManager levelManager;

    public TileDirection currentDirection;

    public float speed;

    public Vector2Int coordinate;

    private void Update()
    {
        Vector2Int oldCoord = coordinate;
        coordinate = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        if (oldCoord.x != coordinate.x || oldCoord.y != coordinate.y)
        {
            OnNewTile?.Invoke(levelManager.GetTile(coordinate));
        }

        levelManager.GetTile(coordinate).ShowDebugLines();

        HandleMoving();
    }

    public bool TrySetDirection(TileDirection dir)
    {
        Tile nextTile = levelManager.GetTile(coordinate, dir);

        // distance check
        if (!IsCloseToCenter()) return false;

        if (MoveableTile(nextTile))
        {
            currentDirection = dir;
            return true;
        }

        return false;
    }

    public bool IsCloseToCenter()
    {
        // distance check
        float dist = Vector2.Distance(transform.position, coordinate);

        if (dist > 0.1f) return false;

        return true;
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
                OnBlocked?.Invoke(levelManager.GetTile(coordinate));
            }
        }
    }
}
