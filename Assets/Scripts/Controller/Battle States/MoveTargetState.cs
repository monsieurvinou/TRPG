public class MoveTargetState : BattleState
{
    /// <summary>
    /// Move the selected tile on move
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="eventInfos">Event informations</param>
    protected override void OnMove(object sender, InfoEventArgs<Point> eventInfos)
    {
        this.SelectTile(eventInfos.info + this.pos);
    }
}
