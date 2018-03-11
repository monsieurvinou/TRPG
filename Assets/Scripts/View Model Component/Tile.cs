using UnityEngine;

public class Tile : MonoBehaviour
{
    public const float stepHeight = 0.25f;
    public Point pos;
    public int height;

    // Convenience property to get the center of the object
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }

    // Method to be called when the scale or position of the tile is modified
    // this way we won't have to do the calculus ourselves
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    // Add 1 to the height
    public void Grow()
    {
        height++;
        Match();
    }

    // Remove 1 to the height
    public void Shrink()
    {
        height--;
        Match();
    }

    // Load a tile with a point and its height
    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }

    // Load a tile with a vector
    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
