using System;
using UnityEngine;

// Перелік можливих станів гри
public enum GameState { Init, Playing, Win, Lose }

public interface IGameStateService
{
    GameState CurrentState { get; }
    event Action<GameState> OnStateChanged;
    void ChangeState(GameState newState);
}

public class GameStateManager : IGameStateService
{
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnStateChanged;

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return; // Захист від подвійного виклику

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}