using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionUI : MonoBehaviour {
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitAction += UnitActionManager_OnSelectedUnitAction;
        UnitActionManager.Instance.OnSelectedActionAction += UnitActionManager_OnSelectedActionAction;
        UnitActionManager.Instance.OnSelectedActionStartedAction += UnitActionManager_OnSelectedActionStartedAction;

        CreateActionButtonUI();
        UpdateActionButtonSelectedVisual();
        UpdateActionPointsText();
    }

    private void UnitActionManager_OnSelectedUnitAction() {
        CreateActionButtonUI();
        UpdateActionButtonSelectedVisual();
        UpdateActionPointsText();
    }

    private void UnitActionManager_OnSelectedActionAction() {
        UpdateActionButtonSelectedVisual();
    }

    private void UnitActionManager_OnSelectedActionStartedAction() {
        UpdateActionPointsText();
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

    private void UpdateActionPointsText() {
        var selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        if (selectedUnit == null) {
            actionPointsText.text = "";
            return;
        }

        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }
}