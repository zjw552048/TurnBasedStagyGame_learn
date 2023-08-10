using TMPro;
using UnityEngine;

public class DebugGridObject : MonoBehaviour {
    [SerializeField] private TextMeshPro textMeshPro;

    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject) {
        this.gridObject = gridObject;
        textMeshPro.text = gridObject.ToString();
    }
}