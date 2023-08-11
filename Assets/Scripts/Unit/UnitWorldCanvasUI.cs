using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldCanvasUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Image healthBarImage;

    [SerializeField] private Unit unit;
    private HealthComponent healthComponent;

    private void Awake() {
        healthComponent = unit.GetComponent<HealthComponent>();
    }

    private void Start() {
        Unit.OnAnyUnitActionPointsChangedAction += Unit_OnAnyUnitActionPointsChangedAction;
        healthComponent.OnHealthChangedAction += HealthComponent_OnHealthChangedAction;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void Unit_OnAnyUnitActionPointsChangedAction() {
        UpdateActionPointsText();
    }

    private void HealthComponent_OnHealthChangedAction() {
        UpdateHealthBar();
    }

    private void UpdateActionPointsText() {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthComponent.GetHealthNormalized();
    }
}