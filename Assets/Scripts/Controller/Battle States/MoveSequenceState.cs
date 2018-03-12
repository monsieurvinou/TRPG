using UnityEngine;
using System.Collections;

/// <summary>
/// DEBUG CLASS TO BE DELETED
/// </summary>
public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        this.StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        AbstractMovement m = this.owner.currentUnit.GetComponent<AbstractMovement>();
        yield return this.StartCoroutine(m.Traverse(this.owner.currentTile));
        this.owner.ChangeState<SelectUnitState>();
    }
}