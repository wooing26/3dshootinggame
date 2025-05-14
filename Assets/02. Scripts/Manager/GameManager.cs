using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over
}

public class GameManager : SingletonBehaviour<GameManager>
{
    private EGameState _gameState = EGameState.Ready;
    public EGameState  GameState  => _gameState;

    public float       ReadyTime  = 3f;
    private float      _gameTimer = 0f;

    private void Start()
    {
        ChangeGameState(EGameState.Ready);
    }

    private void Update()
    {
        if (_gameState != EGameState.Ready)
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
        _gameState = gameState;
        StartCoroutine(UIManager.Instance.ShowGameState(gameState));
    }

    public void Pause()
    {
        _gameState = EGameState.Pause;
        Time.timeScale = 0;

        InputManager.Instance.ChangeCursorState(true);

        PopupManager.Instance.Open(EPopupType.UI_OptionPopup, closeCallback: Continue);
    }

    public void Continue()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        InputManager.Instance.ChangeCursorState(CameraManager.Instance.CameraMode == CameraMode.QuarterView);
    }

    public void Restart()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        InputManager.Instance.ChangeCursorState(false);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
