using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;

    /// <summary>
    /// Place an unit on a tile
    /// </summary>
    /// <param name="target">Target tile</param>
    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (this.tile != null && this.tile.content == this.gameObject)
            this.tile.content = null;

        // Link unit and tile references
        this.tile = target;

        if (target != null)
            target.content = this.gameObject;
    }

    /// <summary>
    /// Match position and direction
    /// </summary>
    public void Match()
    {
        this.transform.localPosition = this.tile.center;
        this.transform.localEulerAngles = this.dir.ToEuler();
    }
}
