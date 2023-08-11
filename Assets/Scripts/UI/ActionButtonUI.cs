using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private Button actionButton;
    [SerializeField] private Image actionSelectedImage;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction) {
        this.baseAction = baseAction;

        actionNameText.text = baseAction.GetActionName().ToUpper();
        actionButton.onClick.AddListener(() => { UnitActionManager.Instance.SetSelectedAction(baseAction); });
    }

    public void UpdateSelectedVisual() {
        var selectedAction = UnitActionManager.Instance.GetSelectedAction();
        actionSelectedImage.enabled = baseAction == selectedAction;
    }
}