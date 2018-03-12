using UnityEngine;
using System.Collections;

/// <summary>
/// DEBUG CLASS TO BE REMOVED
/// </summary>
public class SelectUnitState : BattleState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<string> e)
    {
        GameObject content = owner.currentTile.content;
        if (content != null)
        {
            owner.currentUnit = content.GetComponent<Unit>();
            owner.ChangeState<MoveTargetState>();
        }
    }
}
