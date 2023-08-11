using System;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public static TurnManager Instance { get; private set; }

    public event Action OnTurnChangedAction;

    private int turnNumber;

    private void Awake() {
        turnNumber = 1;

        Instance = this;
    }

    public void EnterNextTurn() {
        turnNumber++;
        OnTurnChangedAction?.Invoke();
    }

    public int GetTurnNumber() {
        return turnNumber;
    }
}