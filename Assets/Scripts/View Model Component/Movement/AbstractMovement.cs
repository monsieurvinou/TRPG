using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractMovement : MonoBehaviour
{
    public int range;
    public int jumpHeight;
    protected Unit unit;
    protected Transform jumper;

    /// <summary>
    /// Awake the class
    /// </summary>
    protected virtual void Awake()
    {
        unit   = this.GetComponent<Unit>();
        jumper = this.transform.Find("Jumper");
    }

    /// <summary>
    /// Method to implement to define how the unit will traverse a tile
    /// </summary>
    /// <param name="tile">Tile to traverse</param>
    /// <returns></returns>
    public abstract IEnumerator Traverse(Tile tile);

    /// <summary>
    /// Find and return all the tiles the unit can reach
    /// </summary>
    /// <param name="board">The board in which we move</param>
    /// <returns></returns>
    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, this.ExpandSearch);
        this.Filter(retValue);
        return retValue;
    }

    /// <summary>
    /// Overridable method to pass as a callback function to search tiles the unit can reach
    /// It will determine if the distance between 2 tiles can be reached
    /// </summary>
    /// <param name="from">Starting tile</param>
    /// <param name="to">Destination tile</param>
    /// <returns>TRUE if the tile is reachable, else FALSE</returns>
    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    /// <summary>
    /// Filter the tiles returned by the search.
    /// This method will remove tiles that can for example be traversed but not stopped at because it
    /// is occupied by an ally or by an obstacle which some unit might be able to fly through/over
    /// </summary>
    /// <param name="tiles">Tiles to filter</param>
    protected virtual void Filter(List<Tile> tiles)
    {
        // for now, we remove all the tiles with something on it
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    /// <summary>
    /// Face the unit in the correct direction to walk
    /// </summary>
    /// <param name="dir">Direction to which we need the unit to be facing</param>
    /// <returns></returns>
    protected virtual IEnumerator Turn(Directions dir)
    {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        unit.dir = dir;

        while (t != null)
            yield return null;
    }
}