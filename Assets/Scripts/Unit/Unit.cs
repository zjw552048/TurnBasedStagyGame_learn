using UnityEngine;

public class Unit : MonoBehaviour {
    private MoveAction moveAction;
    private SpinAction spinAction;
    private GridPosition gridPosition;

    private BaseAction[] baseActionArray;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);
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
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    #region RunAction

    public MoveAction GetMoveAction() {
        return moveAction;
    }

    public SpinAction GetSpineAction() {
        return spinAction;
    }

    public BaseAction GetDefaultAction() {
        return baseActionArray[0];
    }

    #endregion

    public BaseAction[] GetBaseActions() {
        return baseActionArray;
    }
}