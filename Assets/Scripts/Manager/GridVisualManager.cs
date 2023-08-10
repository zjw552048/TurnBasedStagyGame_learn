using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : MonoBehaviour {
    public static GridVisualManager Instance { get; private set; }

    [SerializeField] private Transform gridVisualPrefab;

    private GridVisual[,] gridVisualArray;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InitAllGridVisual();
        HideAllGridVisual();
    }

    private void Update() {
        UpdateGridVisual();
    }

    private void InitAllGridVisual() {
        var width = LevelGrid.Instance.GetWidth();
        var height = LevelGrid.Instance.GetHeight();

        gridVisualArray = new GridVisual[width, height];

        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridVisualTransform = Instantiate(gridVisualPrefab, transform);
                gridVisualTransform.position = LevelGrid.Instance.GetWorldPosition(new GridPosition(x, z));

                var gridVisual = gridVisualTransform.GetComponent<GridVisual>();
                gridVisualArray[x, z] = gridVisual;
            }
        }
    }

    private void HideAllGridVisual() {
        var width = LevelGrid.Instance.GetWidth();
        var height = LevelGrid.Instance.GetHeight();
        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridVisual = gridVisualArray[x, z];
                gridVisual.Hide();
            }
        }
    }

    private void ShowGridVisuals(List<GridPosition> gridPositionList) {
        foreach (var gridPosition in gridPositionList) {
            var gridVisual = gridVisualArray[gridPosition.x, gridPosition.z];
            gridVisual.Show();
        }
    }

    private void UpdateGridVisual() {
        HideAllGridVisual();
        // FIXME: 逻辑实现待优化，每帧都在调用
        var selectAction = UnitActionManager.Instance.GetSelectedAction();
        if (selectAction == null) {
            return;
        }

        var validMoveActionGridPositions = selectAction.GetValidMoveActionGridPositions();
        ShowGridVisuals(validMoveActionGridPositions);
    }
}