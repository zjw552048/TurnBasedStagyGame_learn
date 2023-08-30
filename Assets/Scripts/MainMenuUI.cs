using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private Button playBtn;

    private void Start() {
        playBtn.onClick.AddListener(() => { SceneLoader.LoadScene(SceneLoader.SceneName.GameScene); });
    }
    #region 补丁逻辑，未知原因造成首次进入场景时，UI元素的高度不正确，位置异常。

    private bool hasFixUiPosition;

    private void TryFixUIPosition() {
        if (hasFixUiPosition) {
            return;
        }

        var canvasTransform = (RectTransform) transform.parent;
        if (!(Math.Abs(canvasTransform.rect.height - 1080) < 1)) {
            return;
        }

        Debug.LogWarning("success fix ui position!");
        hasFixUiPosition = true;

        var gameNameTextTransform = (RectTransform) gameNameText.transform;
        gameNameTextTransform.anchoredPosition = new Vector2(150, -150);
        var playerBtnTransform = (RectTransform) playBtn.transform;
        playerBtnTransform.anchoredPosition = new Vector2(150, 210);
    }

    private void Update() {
        TryFixUIPosition();
    }

    #endregion
}