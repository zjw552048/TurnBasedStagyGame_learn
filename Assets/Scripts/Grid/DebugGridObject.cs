using TMPro;
using UnityEngine;

public class DebugGridObject : MonoBehaviour {
    [SerializeField] private TextMeshPro textMeshPro;

    private object gridObject;

    public virtual void SetGridObject(object gridObject) {
        this.gridObject = gridObject;
    }

    protected virtual void Update() {
        textMeshPro.text = gridObject.ToString();
    }
}