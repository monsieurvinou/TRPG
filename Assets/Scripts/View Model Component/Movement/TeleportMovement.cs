using UnityEngine;
using System.Collections;

public class TeleportMovement : AbstractMovement
{
    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);
        Tweener spin = jumper.RotateToLocal(new Vector3(0, 360, 0), 0.5f, EasingEquations.EaseInOutQuad);
        spin.easingControl.loopCount = 1;
        spin.easingControl.loopType = EasingControl.LoopType.PingPong;
        Tweener shrink = transform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);
        while (shrink != null)
            yield return null;
        transform.position = tile.center;
        Tweener grow = transform.ScaleTo(Vector3.one, 0.5f, EasingEquations.EaseOutBack);
        while (grow != null)
            yield return null;
    }
}
