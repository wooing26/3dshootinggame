using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public Slider           PlayerStaminaSlider;
    public Slider           BombThrowPowerSlider;

    public TextMeshProUGUI  BulletText;
    public TextMeshProUGUI  BombText;

    public Image            ReloadImage;

    public void RefreshPlayerStaminaSlider(float currentStamina, float MaxStamina)
    {
        PlayerStaminaSlider.value = currentStamina / MaxStamina;
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
