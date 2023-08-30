using UnityEngine;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour {
    [SerializeField] private Button playBtn;

    private void Start() {
        playBtn.onClick.AddListener(() => { SceneLoader.LoadScene(SceneLoader.SceneName.GameScene); });
    }
}