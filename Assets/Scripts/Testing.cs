using UnityEngine;

public class Testing : MonoBehaviour {
    [SerializeField] private Transform debugGridPrefab;
    private GridSystem gridSystem;

    private void Start() {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugGrid(debugGridPrefab);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        }
    }
}