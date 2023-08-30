using System;
using Enum;
using UnityEngine;

public class Unit : MonoBehaviour {
    public static event Action OnAnyUnitActionPointsChangedAction;
    public static event Action OnAnyUnitGridPositionChangedAction;
    public static event Action<Unit> OnAnyUnitSpawnAction;
    public static event Action<Unit> OnAnyUnitDestroyAction;

    public static void ResetStaticData() {
        OnAnyUnitActionPointsChangedAction = null;
        OnAnyUnitGridPositionChangedAction = null;
        OnAnyUnitSpawnAction = null;
        OnAnyUnitDestroyAction = null;
    }

    [SerializeField] private bool isPlayer;

    public const float HEIGHT_OFFSET = 1.7f;
    public const float SHOULDER_OFFSET = 0.5f;

    private GridPosition gridPosition;

    private BaseAction[] baseActionArray;

    private int actionPoints;
    private const int MAX_ACTION_POINTS = 10;

    private HealthComponent healthComponent;

    private void Awake() {
        baseActionArray = GetComponents<BaseAction>();
        healthComponent = GetComponent<HealthComponent>();

        actionPoints = MAX_ACTION_POINTS;
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);

        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;
        healthComponent.OnHealthZeroAction += HealthComponent_OnHealthZeroAction;

        OnAnyUnitSpawnAction?.Invoke(this);
    }

    private void TurnManager_OnTurnChangedAction() {
        ResetActionPoints();
    }

    private void HealthComponent_OnHealthZeroAction(object sender, HealthComponent.OnHealthZeroActionArgs args) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(this, gridPosition);
        Destroy(gameObject);

        OnAnyUnitDestroyAction?.Invoke(this);
    }

    private void Update() {
        HandleGridRefresh();
    }

    private void HandleGridRefresh() {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition == gridPosition) {
            return;
        }

        LevelGrid.Instance.MoveUnitGridPosition(this, gridPosition, newGridPosition);
        gridPosition = newGridPosition;

        OnAnyUnitGridPositionChangedAction?.Invoke();
    }

    #region 获取unit信息

    public Vector3 GetWorldPosition() {
        return transform.position;
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public BaseAction GetDefaultAction() {
        return baseActionArray[0];
    }

    public BaseAction[] GetBaseActions() {
        return baseActionArray;
    }

    public T GetAction<T>() where T : BaseAction {
        foreach (var baseAction in baseActionArray) {
            if (baseAction is T) {
                return (T) baseAction;
            }
        }

        return null;
    }

    public bool IsPlayer() {
        return isPlayer;
    }

    public HealthComponent GetHealthComponent() {
        return healthComponent;
    }

    #endregion

    #region actionPoints

    private void ResetActionPoints() {
        if (isPlayer && TurnManager.Instance.IsPlayerTurn() ||
            !isPlayer && !TurnManager.Instance.IsPlayerTurn()) {
            actionPoints = MAX_ACTION_POINTS;
            OnAnyUnitActionPointsChangedAction?.Invoke();
        }
    }

    public int GetActionPoints() {
        return actionPoints;
    }

    private void SpendActionPointsToTakeAction(BaseAction action) {
        actionPoints -= action.NeedCostActionPoints();
        OnAnyUnitActionPointsChangedAction?.Invoke();
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction action) {
        return actionPoints >= action.NeedCostActionPoints();
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action) {
        if (!CanSpendActionPointsToTakeAction(action)) {
            return false;
        }

        SpendActionPointsToTakeAction(action);
        return true;
    }

    #endregion
}