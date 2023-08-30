using UnityEngine;

public class LoadingScene : MonoBehaviour {
    private void Start() {
        SceneLoader.LoadingSceneEnterCallback();
    }
}