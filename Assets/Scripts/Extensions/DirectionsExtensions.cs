using UnityEngine;

public static class DirectionsExtensions
{
    /// <summary>
    /// Get the direction from a tile to another
    /// </summary>
    /// <param name="tileA">Tile of origin</param>
    /// <param name="tileB">Tile of destination</param>
    /// <returns></returns>
    public static Directions GetDirection(this Tile tileA, Tile tileB)
    {
        if (tileA.pos.y < tileB.pos.y) return Directions.North;
        else if (tileA.pos.x < tileB.pos.x) return Directions.East;
        else if (tileA.pos.y > tileB.pos.y) return Directions.South;
        else return Directions.West;
    }

    /// <summary>
    /// Convert a direction to Euler
    /// </summary>
    /// <param name="direction">The direction</param>
    /// <returns></returns>
    public static Vector3 ToEuler(this Directions direction)
    {
        return new Vector3(0, (int)direction * 90, 0);
    }
}
