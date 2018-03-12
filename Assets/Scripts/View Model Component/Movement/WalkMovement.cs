using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkMovement : AbstractMovement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        bool canTravel = base.ExpandSearch(from, to);

        if (canTravel)
        {
            if ((Mathf.Abs(from.height - to.height) > this.jumpHeight))
                canTravel = false;
            else if (to.content != null)
                canTravel = false;
        }

        return canTravel;
    }

    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);
        // Build a list of way points from the unit's 
        // starting tile to the destination tile
        List<Tile> targets = new List<Tile>();

        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
        }

        // Move to each way point in succession
        for (int i = 1; i < targets.Count; ++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];
            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
                yield return StartCoroutine(Turn(dir));
            if (from.height == to.height)
                yield return StartCoroutine(Walk(to));
            else
                yield return StartCoroutine(Jump(to));
        }

        yield return null;
    }

    /// <summary>
    /// Animation coroutine making the unit walk
    /// </summary>
    /// <param name="target">Destination of the unit</param>
    /// <returns></returns>
    IEnumerator Walk(Tile target)
    {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);

        while (tweener != null)
            yield return null;
    }

    /// <summary>
    /// Animation coroutine making the unit jump
    /// </summary>
    /// <param name="to">Destination of the unit</param>
    /// <returns></returns>
    IEnumerator Jump(Tile to)
    {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);
        Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
        t2.easingControl.loopCount = 1;
        t2.easingControl.loopType = EasingControl.LoopType.PingPong;

        while (tweener != null)
            yield return null;
    }
}
