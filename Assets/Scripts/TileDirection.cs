using UnityEngine;

public enum TileDirection
{
    UP,
    RIGHT,
    DOWN,
    LEFT,
    NONE
}

public static class TileDirectionEnum
{
    public static TileDirection Get_TD(Vector2Int from, Vector2Int to)
    {
        Vector2Int diff = to - from;

        if (diff.x > 0) return TileDirection.RIGHT;
        if (diff.x < 0) return TileDirection.LEFT;
        if (diff.y > 0) return TileDirection.UP;
        if (diff.y < 0) return TileDirection.DOWN;

        // default
        return TileDirection.NONE;
    }
}

public static class TileDirectionVec2
{
    public readonly static Vector2Int UP_V2I = new Vector2Int(0, 1);

    public readonly static Vector2Int RIGHT_V2I = new Vector2Int(1, 0);

    public readonly static Vector2Int DOWN_V2I = new Vector2Int(0, -1);

    public readonly static Vector2Int LEFT_V2I = new Vector2Int(-1, 0);

    private readonly static Vector2Int[] ARRAY_V2I = { UP_V2I, RIGHT_V2I, DOWN_V2I, LEFT_V2I, Vector2Int.zero};

    private readonly static Vector2[] ARRAY_V2F = { UP_V2I, RIGHT_V2I, DOWN_V2I, LEFT_V2I, Vector2.zero };

    public static Vector2Int Get_V2I(TileDirection dir)
    {
        return ARRAY_V2I[(int)dir];
    }

    public static Vector2 Get_V2F(TileDirection dir)
    {
        return ARRAY_V2F[(int)dir];
    }
}
