using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTargetState : BattleState
{
    List<Tile> tiles;

    public override void Enter()
    {
        base.Enter();
        AbstractMovement mover = this.owner.currentUnit.GetComponent<AbstractMovement>();
        this.tiles = mover.GetTilesInRange(this.board);
        this.board.SelectTiles(this.tiles);
    }

    public override void Exit()
    {
        base.Exit();
        this.board.DeSelectTiles(this.tiles);
        tiles = null;
    }

    /// <summary>
    /// Move the selected tile on move
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="eventInfos">Event informations</param>
    protected override void OnMove(object sender, InfoEventArgs<Point> eventInfos)
    {
        this.SelectTile(eventInfos.info + this.pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<string> e)
    {
        if (this.tiles.Contains(this.owner.currentTile))
            this.owner.ChangeState<MoveSequenceState>();
    }
}
