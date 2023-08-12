using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance { get; private set; }
    private List<Unit> unitList;
    private List<Unit> enemyUnitList;
    private List<Unit> playerUnitList;

    private void Awake() {
        unitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        playerUnitList = new List<Unit>();

        Instance = this;
    }

    private void Start() {
        Unit.OnAnyUnitSpawnAction += Unit_OnAnyUnitSpawnAction;
        Unit.OnAnyUnitDestroyAction += Unit_OnAnyUnitDestroyAction;
    }

    private void Unit_OnAnyUnitSpawnAction(Unit unit) {
        unitList.Add(unit);
        if (unit.IsPlayer()) {
            playerUnitList.Add(unit);
        } else {
            enemyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDestroyAction(Unit unit) {
        unitList.Remove(unit);
        if (unit.IsPlayer()) {
            playerUnitList.Remove(unit);
        } else {
            enemyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList() {
        return unitList;
    }

    public List<Unit> GetEnemyUnitList() {
        return enemyUnitList;
    }

    public List<Unit> GetPlayerUnitList() {
        return playerUnitList;
    }
}