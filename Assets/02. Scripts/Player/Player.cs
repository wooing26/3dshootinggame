using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    public int      Health = 100;
    public Image    BloodScreenImage;
    public float    BloodScreenTime = 1f;

    private Coroutine _bloodScreenCoroutine;
    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        
        if (_bloodScreenCoroutine != null)
        {
            StopCoroutine(_bloodScreenCoroutine);
        }
        _bloodScreenCoroutine = StartCoroutine(OnBloodScreen_Coroutine());
        
        Debug.Log(Health);
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator OnBloodScreen_Coroutine()
    {
        BloodScreenImage.gameObject.SetActive(true);

        float timer = BloodScreenTime;
        Color color = BloodScreenImage.color;
        color.a = 1;

        while (timer >= 0)
        {
            color.a = timer / BloodScreenTime;
            timer -= Time.deltaTime;
            BloodScreenImage.color = color;
            yield return null;
        }

        BloodScreenImage.gameObject.SetActive(false);
    }
}
