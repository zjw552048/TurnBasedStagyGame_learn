using UnityEngine;
using UnityEngine.UI;

public class GameFailUI : MonoBehaviour {
    [SerializeField] private Button backMenuBtn;

    private void Start() {
        backMenuBtn.onClick.AddListener(() => { SceneLoader.LoadScene(SceneLoader.SceneName.MainMenuScene); });
        UnitManager.Instance.OnAllPlayerUnitLostAction += OnAllPlayerUnitLostAction;
        Hide();
    }

    private void OnAllPlayerUnitLostAction() {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}