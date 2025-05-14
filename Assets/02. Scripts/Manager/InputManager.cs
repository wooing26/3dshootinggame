using UnityEngine;

public class InputManager : SingletonBehaviour<InputManager>
{

    private void Start()
    {
        ChangeCursorState(false);
        CameraManager.Instance.OnChangeCameraMode += ChangeCameraMode;
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        ChangeCursorState(cameraMode == CameraMode.QuarterView);
    }

    public void ChangeCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ChangeCursorState(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            ChangeCursorState(false);
        }
    }

    public Vector2 GetMousePositionFromCenter()
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return Vector2.zero;
        }
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 mousePosition = Input.mousePosition;

        return mousePosition - screenCenter;
    }

    public float GetAxis(string axis)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return 0f;
        }

        return Input.GetAxis(axis);
    }

    public float GetAxisRaw(string axis)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return 0f;
        }

        return Input.GetAxisRaw(axis);
    }

    public bool GetKey(KeyCode keyCode)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetKey(keyCode);
    }

    public bool GetKeyDown(KeyCode keyCode)
    {
        if (GameManager.Instance.GameState == EGameState.Ready || GameManager.Instance.GameState == EGameState.Over)
        {
            return false;
        }

        return Input.GetKeyDown(keyCode);
    }

    public bool GetKeyUp(KeyCode keyCode)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetKeyUp(keyCode);
    }
    public bool GetButton(string buttonName)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetButton(buttonName);
    }

    public bool GetButtonDown(string buttonName)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetButtonDown(buttonName);
    }

    public bool GetButtonUp(string buttonName)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetButtonUp(buttonName);
    }

    public bool GetMouseButton(int button)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetMouseButton(button);
    }

    public bool GetMouseButtonDown(int button)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetMouseButtonDown(button);
    }

    public bool GetMouseButtonUp(int button)
    {
        if (GameManager.Instance.GameState != EGameState.Run)
        {
            return false;
        }

        return Input.GetMouseButtonUp(button);
    }
}
