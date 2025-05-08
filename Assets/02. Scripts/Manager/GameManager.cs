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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        PopupManager.Instance.Open(EPopupType.UI_OptionPopup, closeCallback: Continue);
    }

    public void Continue()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
