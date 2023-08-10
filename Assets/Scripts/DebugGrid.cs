using TMPro;
using UnityEngine;

public class DebugGrid : MonoBehaviour {
    [SerializeField] private TextMeshPro textMeshPro;

    public void SetText(GridPosition gridPosition) {
        textMeshPro.text = $"{gridPosition.x}, {gridPosition.z}";
    }
}