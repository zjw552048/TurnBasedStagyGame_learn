using System;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : MonoBehaviour {
    public static GridVisualManager Instance { get; private set; }

    private enum GridVisualColorType {
        While,
        Red,
        RedSoft,
        Yellow,
        Blue,
    }

    [Serializable]
    private struct GridVisualColorMatchMaterial {
        public GridVisualColorType gridVisualColorType;
        public Material gridVisualMaterial;
    }

    [SerializeField] private Transform gridVisualPrefab;
    [SerializeField] private List<GridVisualColorMatchMaterial> gridVisualColorMatchMaterials;

    private GridVisual[,] gridVisualArray;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        UnitActionManager.Instance.OnSelectedActionAction += UnitActionManager_OnSelectedActionAction;
        Unit.OnAnyUnitGridPositionChangedAction += Unit_OnAnyUnitGridPositionChangedAction;
        Unit.OnAnyUnitDestroyAction += UnitOnOnAnyUnitDestroyAction;

        InitAllGridVisual();
        HideAllGridVisual();
    }

    private void UnitActionManager_OnSelectedActionAction() {
        UpdateGridVisual();
    }

    private void Unit_OnAnyUnitGridPositionChangedAction() {
        UpdateGridVisual();
    }

    private void UnitOnOnAnyUnitDestroyAction(Unit obj) {
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

    private void ShowGridVisuals(List<GridPosition> gridPositionList, Material material) {
        foreach (var gridPosition in gridPositionList) {
            var gridVisual = gridVisualArray[gridPosition.x, gridPosition.z];
            gridVisual.Show(material);
        }
    }

    private void UpdateGridVisual() {
        HideAllGridVisual();

        var selectAction = UnitActionManager.Instance.GetSelectedAction();
        if (selectAction == null) {
            return;
        }

        var gridVisualColorType = GridVisualColorType.While;
        switch (selectAction) {
            case MoveAction moveAction:
                gridVisualColorType = GridVisualColorType.While;
                break;

            case SpinAction spinAction:
                gridVisualColorType = GridVisualColorType.Blue;
                break;

            case ShootAction shootAction:
                // 显示ShootRange范围内的GridVisual
                var shootRangeGridPositions = shootAction.GetValidActionRangeGridPositions();
                ShowGridVisuals(shootRangeGridPositions, GetGridVisualMaterialByColor(GridVisualColorType.RedSoft));

                gridVisualColorType = GridVisualColorType.Red;
                break;

            case GrenadeAction grenadeAction:
                gridVisualColorType = GridVisualColorType.Yellow;
                break;

            case SwordAction swordAction:
                // 显示ShootRange范围内的GridVisual
                var swordRangeGridPositions = swordAction.GetValidActionRangeGridPositions();
                ShowGridVisuals(swordRangeGridPositions, GetGridVisualMaterialByColor(GridVisualColorType.RedSoft));

                gridVisualColorType = GridVisualColorType.Red;
                break;
        }

        var material = GetGridVisualMaterialByColor(gridVisualColorType);

        var validMoveActionGridPositions = selectAction.GetValidActionGridPositions();
        ShowGridVisuals(validMoveActionGridPositions, material);
    }

    private Material GetGridVisualMaterialByColor(GridVisualColorType gridVisualColorType) {
        foreach (var gridVisualColorMatchMaterial in gridVisualColorMatchMaterials) {
            if (gridVisualColorMatchMaterial.gridVisualColorType == gridVisualColorType) {
                return gridVisualColorMatchMaterial.gridVisualMaterial;
            }
        }

        return null;
    }
}