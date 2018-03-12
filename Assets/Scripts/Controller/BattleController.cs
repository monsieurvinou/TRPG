using UnityEngine;

public class BattleController : StateMachine
{
    // Level and camera
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    // Units
    public GameObject heroPrefab;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    /// <summary>
    /// Unity start
    /// </summary>
    void Start()
    {
        ChangeState<InitBattleState>();
    }
}
