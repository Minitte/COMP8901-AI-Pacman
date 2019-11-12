using UnityEngine;

public enum TileDirection
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public static class TileDirectionVec2
{
    public readonly static Vector2Int UP_V2I = new Vector2Int(0, 1);

    public readonly static Vector2Int RIGHT_V2I = new Vector2Int(1, 0);

    public readonly static Vector2Int DOWN_V2I = new Vector2Int(0, -1);

    public readonly static Vector2Int LEFT_V2I = new Vector2Int(-1, 0);

    private readonly static Vector2Int[] ARRAY_V2I = { UP_V2I, RIGHT_V2I, DOWN_V2I, LEFT_V2I };

    private readonly static Vector2[] ARRAY_V2F = { UP_V2I, RIGHT_V2I, DOWN_V2I, LEFT_V2I };

    public static Vector2Int Get_V2I(TileDirection dir)
    {
        return ARRAY_V2I[(int)dir];
    }

    public static Vector2 Get_V2F(TileDirection dir)
    {
        return ARRAY_V2F[(int)dir];
    }
}
