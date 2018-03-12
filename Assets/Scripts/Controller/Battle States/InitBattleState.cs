using UnityEngine;
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
        // Test code -start-
        SpawnTestUnits();
        // Test code -end-
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

// DEBUG CODE TO BE REMOVED
    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] {
            typeof(WalkMovement),
            typeof(FlyMovement),
            typeof(TeleportMovement)
        };

        for (int i = 0; i < 3; ++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(this.board.GetTile(p));
            unit.Match();
            AbstractMovement m = instance.AddComponent(components[i]) as AbstractMovement;
            m.range = 5;
            m.jumpHeight = 1;
        }
    }
}
