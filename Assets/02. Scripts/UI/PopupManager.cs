using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup
}

public class PopupManager : SingletonBehaviour<PopupManager>
{
    public List<UI_Popup>   Popups;
    private Stack<UI_Popup> _openedPopups = new Stack<UI_Popup>();

    public void Open(EPopupType type, Action closeCallback = null)
    {
        Open(type.ToString(), closeCallback);
    }

    private void Open(string popupName, Action closeCallback)
    {
        foreach(UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                _openedPopups.Push(popup);
                break;
            }
        }
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count == 0)
            {
                GameManager.Instance.Pause();
                return;
            }

            Close();
        }
    }

    public void Close()
    {
        while (true)
        {
            UI_Popup popup = _openedPopups.Peek();
            bool opened = popup.isActiveAndEnabled;
            _openedPopups.Pop();

            if (opened)
            {
                popup.Close();
                break;
            }
            
            if (_openedPopups.Count == 0)
            {
                break;
            }
        }

        Debug.Log(_openedPopups.Count);
    }

    public void Clear()
    {
        while (true)
        {
            UI_Popup popup = _openedPopups.Peek();
            bool opened = popup.isActiveAndEnabled;
            _openedPopups.Pop();

            if (opened)
            {
                popup.Close();
            }

            if (_openedPopups.Count == 0)
            {
                break;
            }
        }
    }
}
