using UnityEngine.SceneManagement;

public static class SceneLoader {
    public enum SceneName {
        MainMenuScene,
        LoadingScene,
        GameScene,
    }

    private static SceneName targetSceneName;

    public static void LoadScene(SceneName sceneName) {
        targetSceneName = sceneName;
        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }


    public static void LoadingSceneEnterCallback() {
        SceneManager.LoadScene(targetSceneName.ToString());
    }
}