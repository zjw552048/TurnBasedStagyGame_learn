using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private Button actionButton;

    private BaseAction baseAction;


    public void SetBaseAction(BaseAction baseAction) {
        this.baseAction = baseAction;
        UpdateUI();
    }

    private void UpdateUI() {
        actionNameText.text = baseAction.GetActionName().ToUpper();
        actionButton.onClick.AddListener(() => { });
    }
}