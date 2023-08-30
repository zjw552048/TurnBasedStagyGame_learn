using UnityEngine;

public class MouseWorld : MonoBehaviour {
    public static MouseWorld Instance { get; private set; }

    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        transform.position = GetPosition();
    }

    public static Vector3 GetPosition() {
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        Physics.Raycast(ray, out var hitInfo, float.MaxValue, Instance.mousePlaneLayerMask);
        return hitInfo.point;
    }
}