using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionUI : MonoBehaviour {
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitAction += UnitActionManager_OnSelectedUnitAction;
        UnitActionManager.Instance.OnSelectedActionAction += UnitActionManager_OnSelectedActionAction;
        CreateActionButtonUI();
    }

    private void UnitActionManager_OnSelectedUnitAction() {
        CreateActionButtonUI();
    }

    private void UnitActionManager_OnSelectedActionAction() {
        UpdateActionButtonSelectedVisual();
    }

    private void CreateActionButtonUI() {
        foreach (Transform childTransform in actionButtonContainerTransform) {
            Destroy(childTransform.gameObject);
        }

        actionButtonUIList.Clear();

        var selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        if (selectedUnit == null) {
            return;
        }

        var baseActionArray = selectedUnit.GetBaseActions();
        foreach (var baseAction in baseActionArray) {
            var actionButtonUiTransform = Instantiate(actionButtonUIPrefab, actionButtonContainerTransform);
            var actionButtonUI = actionButtonUiTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UpdateActionButtonSelectedVisual() {
        foreach (var actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}