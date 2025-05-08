using System;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    // 콜백 함수 : 어떤 함수를 기억해놨다가 특정 시점(작업이 완료된 후)에 호출하는 함수
    private Action _cloaseCallback;

    public void Open(Action cloaseCallback = null)  // 1. 팝업 열고, 닫힐 때 호출할 함수 등록
    {
        _cloaseCallback = cloaseCallback;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        _cloaseCallback?.Invoke();                  // 2. 닫힐 때 함수가 등록되어있으면 실행

        gameObject.SetActive(false);
    }
}
