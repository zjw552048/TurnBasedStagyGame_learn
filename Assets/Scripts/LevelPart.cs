using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour {
    [SerializeField] private List<GameObject> part2ShadowList;
    [SerializeField] private List<GameObject> part3ShadowList;

    [SerializeField] private List<GameObject> part1UnitList;
    [SerializeField] private List<GameObject> part2UnitList;
    [SerializeField] private List<GameObject> part3UnitList;

    [SerializeField] private Door door1;
    [SerializeField] private Door door2;

    private void Start() {
        ChangeGameObjectActive(part1UnitList, true);
        door1.OnDoorOpenAction += () => {
            ChangeGameObjectActive(part2ShadowList, false);
            ChangeGameObjectActive(part2UnitList, true);
        };
        door2.OnDoorOpenAction += () => {
            ChangeGameObjectActive(part3ShadowList, false);
            ChangeGameObjectActive(part3UnitList, true);
        };
    }

    private void ChangeGameObjectActive(List<GameObject> gameObjects, bool active) {
        foreach (var gameObject in gameObjects) {
            gameObject.SetActive(active);
        }
    }
}