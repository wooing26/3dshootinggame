using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    private Player          _player;

    public Slider           PlayerHealthSlider;
    public Slider           PlayerStaminaSlider;
    public Slider           BombThrowPowerSlider;

    public TextMeshProUGUI  BulletText;
    public TextMeshProUGUI  BombText;

    public Image            ReloadImage;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _player.OnChangePlayerStat += RefreshPlayerSlider;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
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
}
