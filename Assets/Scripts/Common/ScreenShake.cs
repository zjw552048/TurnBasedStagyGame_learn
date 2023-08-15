using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake() {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        Instance = this;
    }


    public void ScreenShakeWithForce(float force = 1f) {
        cinemachineImpulseSource.GenerateImpulseWithForce(force);
    }
}