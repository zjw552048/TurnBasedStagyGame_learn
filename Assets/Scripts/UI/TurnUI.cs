using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnBtn;

    private void Start() {
        endTurnBtn.onClick.AddListener(() => { TurnManager.Instance.EnterNextTurn(); });

        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;

        UpdateTurnNumber();
    }

    private void TurnManager_OnTurnChangedAction() {
        UpdateTurnNumber();
    }

    private void UpdateTurnNumber() {
        turnNumberText.text = "Turn " + TurnManager.Instance.GetTurnNumber();
    }
}