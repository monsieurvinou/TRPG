using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController owner;
    public CameraRig cameraRig { get { return owner.cameraRig; } }
    public Board board { get { return owner.board; } }
    public LevelData levelData { get { return owner.levelData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }

    /// <summary>
    /// When awaken, define the owner of the battle state
    /// </summary>
    protected virtual void Awake()
    {
        owner = GetComponent<BattleController>();
    }

    /// <summary>
    /// Add the event listeners
    /// </summary>
    protected override void AddListeners()
    {
        InputController.MoveEvent += OnMove;
        InputController.FireEvent += OnFire;
    }

    /// <summary>
    /// Remove the event listeners
    /// </summary>
    protected override void RemoveListeners()
    {
        InputController.MoveEvent -= OnMove;
        InputController.FireEvent -= OnFire;
    }

    /// <summary>
    /// Actions to execute when moving
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="eventInfos">Event informations</param>
    protected virtual void OnMove(object sender, InfoEventArgs<Point> eventInfos)
    {

    }

    /// <summary>
    /// Actions to execute when using a button
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="eventInfos">Event informations</param>
    protected virtual void OnFire(object sender, InfoEventArgs<string> eventInfos)
    {

    }

    /// <summary>
    /// Select a tile at a coordinate
    /// </summary>
    /// <param name="point">The coordinates</param>
    protected virtual void SelectTile(Point point)
    {
        if (this.pos != point)
        {
            if (this.board.tiles.ContainsKey(point))
            {
                this.pos = point;
                this.tileSelectionIndicator.localPosition = this.board.tiles[point].center;
            }
        }
    }
}
