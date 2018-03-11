using System.Collections;

public class InitBattleState : BattleState
{
    /// <summary>
    /// Enter Init Battle State
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    /// <summary>
    /// Coroutine of initialization
    /// </summary>
    /// <returns></returns>
    IEnumerator Init()
    {
        this.board.Load(this.levelData);
        Point selectedTile = new Point(
            (int)this.levelData.tiles[0].x,
            (int)this.levelData.tiles[0].z
        );

        SelectTile(selectedTile);
        yield return null;
        owner.ChangeState<MoveTargetState>();
    }
}
