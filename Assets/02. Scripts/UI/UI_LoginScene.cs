using DG.Tweening;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;                  // 결과 텍스트
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordComfirmInputField;
    public Button ConfirmButton;                        // 로그인 or 회원가입 버튼
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject     LoginPanel;
    public GameObject     RegisterPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;

    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    [Header("트위닝")]
    public float          WarningShakeTime      = 1f;
    public float          WarningShakeMagnitude = 5f;

    private const string  PREFIX                = "ID_";
    private const string  SALT                  = "981226";

    private void Start()
    {
        OnClickGoToLoginButton();

        LoginCheck();
    }

    public void OnClickGoToRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        RegisterInputFields.ResultText.enabled = false;

        RegisterInputFields.IDInputField.text = string.Empty;
        RegisterInputFields.PasswordInputField.text = string.Empty;
        RegisterInputFields.PasswordComfirmInputField.text = string.Empty;
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        LoginInputFields.ResultText.enabled = false;
    }

    // 회원가입
    public void Register()
    {
        // 1. 아이디 입력을 확인한다.
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            ShowResultText(RegisterInputFields, "아이디를 입력해 주세요.");
            return;
        }

        // 2. 비밀번호를 입력한다.
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            ShowResultText(RegisterInputFields, "패스워드를 입력해 주세요.");
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string passwordComfirm = RegisterInputFields.PasswordComfirmInputField.text;
        if (string.IsNullOrEmpty(passwordComfirm))
        {
            ShowResultText(RegisterInputFields, "패스워드를 입력해 주세요.");
            return;
        }
        if (passwordComfirm != password)
        {
            ShowResultText(RegisterInputFields, "패스워드와 패스워드 확인 값이 다릅니다..");
            return;
        }

        // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다.
        // 비밀번호를 암호화 해서 저장
        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));

        // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동 입력되어 있다.)
        LoginInputFields.IDInputField.text = id;
        OnClickGoToLoginButton();
    }

    // 로그인
    public void Login()
    {
        // 1. 아이디 입력을 확인한다.
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            ShowResultText(LoginInputFields, "아이디를 입력해 주세요.");
            return;
        }

        // 2. 비밀번호를 입력한다.
        string password = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            ShowResultText(LoginInputFields, "패스워드를 입력해 주세요.");
            return;
        }

        // 3. PlayerPrefs.Get을 이용해서 아이디와 비밀번호가 맞는지 확인한다.
        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            ShowResultText(LoginInputFields, "아이디와 비밀번호를 확인해 주세요.");
            return;
        }

        string correctPassword = PlayerPrefs.GetString(PREFIX + id);
        if (correctPassword != Encryption(password + SALT))
        {
            ShowResultText(LoginInputFields, "아이디와 비밀번호를 확인해 주세요.");
            return;
        }

        // 4. 맞다면 로그인
        Debug.Log("로그인 성공");
        SceneManager.LoadScene(1);
    }

    private void ShowResultText(UI_InputFields inputFields, string resultText)
    {
        if (!inputFields.ResultText.enabled)
        {
            inputFields.ResultText.enabled = true;
        }
        inputFields.ResultText.text = resultText;
        inputFields.ResultText.rectTransform.DOShakePosition(WarningShakeTime, WarningShakeMagnitude);
    }

    public string Encryption(string text)
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();

        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // UTF8 버전 바이트로 배열로 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach(byte b in hash)
        {
            // byte를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("X2");
        }

        return resultText;
    }

    //private void Update()
    //{
    //    LoginCheck();
    //}

    // 아이디와 비밀번호 InputField 값이 바뀌었을 경우에만 호출
    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
