using UnityEngine;

public class InputManager : SingletonBehaviour<InputManager>
{

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
        if (GameManager.Instance.GameState != EGameState.Run)
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
