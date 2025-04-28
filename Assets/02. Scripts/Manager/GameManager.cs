using UnityEngine;

public enum EGameState
{
    Ready,
    Run,
    Over
}

public class GameManager : SingletonBehaviour<GameManager>
{
    public EGameState   GameState = EGameState.Ready;

    public float        ReadyTime = 3f;
    private float       _gameTimer = 0f;

    private void Start()
    {
        GameState = EGameState.Ready;
    }

    private void Update()
    {
        if (GameState != EGameState.Ready)
        {
            return;
        }

        _gameTimer += Time.deltaTime;
        if (_gameTimer >= ReadyTime)
        {
            ChangeGameState(EGameState.Run);
        }
    }

    public void ChangeGameState(EGameState gameState)
    {
        GameState = gameState;
    }
}
