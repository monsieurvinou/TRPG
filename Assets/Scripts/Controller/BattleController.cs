using UnityEngine;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    /// <summary>
    /// Unity start
    /// </summary>
    void Start()
    {
        ChangeState<InitBattleState>();
    }
}
