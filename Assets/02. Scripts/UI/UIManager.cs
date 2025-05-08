using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    private Player         _player;

    public Slider          PlayerHealthSlider;
    public Slider          PlayerStaminaSlider;
    public Slider          BombThrowPowerSlider;

    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI BombText;
    public TextMeshProUGUI GameStateText;

    public Image           ReloadImage;
    public Image           CrosshairImage;

    public GameObject      OptionPopup;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _player.OnChangePlayerStat += RefreshPlayerSlider;
    }

    public IEnumerator ShowGameState(EGameState gameState)
    {
        string text = "";
        switch (gameState)
        {
            case EGameState.Ready:
                {
                    text = "Ready...";
                    break;
                }
            case EGameState.Run:
                {
                    text = "GO!!!";
                    break;
                }
            case EGameState.Over:
                {
                    text = "Game Over";
                    break;
                }
        }

        GameStateText.text = text;
        GameStateText.enabled = true;

        yield return new WaitForSeconds(1f);
        GameStateText.enabled = false;
    }

    public void RefreshPlayerSlider()
    {
        PlayerHealthSlider.value = (float)_player.CurrentHealth / _player.MaxHealth;
        PlayerStaminaSlider.value = _player.CurrentStamina / _player.MaxStamina;
    }

    public void RefreshBombThrowPowerSlider(float currentThrowPower, float MaxThrowPower)
    {
        BombThrowPowerSlider.value = currentThrowPower / MaxThrowPower;
    }

    public void RefreshBulletText(int currentBulletCount, int maxBulletCount)
    {
        BulletText.text = $"{currentBulletCount} / {maxBulletCount}";
    }

    public void RefreshBombText(int currentBombCount, int maxBombCount)
    {
        BombText.text = $"{currentBombCount} / {maxBombCount}";
    }

    public void RefreshReloadImage(float reloadTimer, float reloadTime)
    {
        ReloadImage.fillAmount = reloadTimer / reloadTime;
    }

    public void SetReloadImageActive(bool active)
    {
        ReloadImage.enabled = active;
    }

    public void SetCrosshairImageActive(bool active)
    {
        CrosshairImage.enabled = active;
    }

    public void SetOptionPopupActive(bool active)
    {
        OptionPopup.SetActive(active);
    }
}
