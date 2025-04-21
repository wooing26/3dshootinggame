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

    public Slider PlayerStaminaSlider;

    public void RefreshPlayerStaminaSlider(float currentStamina, float MaxStamina)
    {
        PlayerStaminaSlider.value = currentStamina / MaxStamina;
    }
}
