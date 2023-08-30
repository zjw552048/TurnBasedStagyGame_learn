using UnityEngine;

public class MobileInputUI : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}