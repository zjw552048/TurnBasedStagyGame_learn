using System;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public static TurnManager Instance { get; private set; }

    public event Action OnTurnChangedAction;

    private int turnNumber;
    private bool isPlayerTurn;

    private void Awake() {
        turnNumber = 1;
        isPlayerTurn = true;

        Instance = this;
    }

    public void EnterNextTurn() {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChangedAction?.Invoke();
    }

    public int GetTurnNumber() {
        return turnNumber;
    }

    public bool IsPlayerTurn() {
        return isPlayerTurn;
    }
}