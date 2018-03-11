using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point
{
    public int x;
    public int y;

    /// <summary>
    /// Point contructor
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public Point (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // ------------------------------------------------
    // Operators overrides
    // ------------------------------------------------
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }
    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }
    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }
    public static implicit operator Vector2(Point p)
    {
        return new Vector2(p.x, p.y);
    }

    // ------------------------------------------------
    // Object overrides
    // ------------------------------------------------
    public override bool Equals(object obj)
    {
        bool equals = false;

        if (obj is Point)
        {
            Point p = (Point)obj;
            equals = x == p.x && y == p.y;
        }

        return equals;
    }
    public bool Equals(Point p)
    {
        return x == p.x && y == p.y;
    }
    public override int GetHashCode()
    {
        return x ^ y;
    }
    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}
