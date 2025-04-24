using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int          Health = 30;
    private bool        _isDead = false;

    public GameObject   ExplosionEffectPrefab;

    public float        ExplosionRadius = 10f;
    public int          ExplosionDamage = 10;
    private int         _damagedLayerMask = 0;

    public float        LifeTime = 3f;
    public float        FlyingPower = 100f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        _damagedLayerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy");
        _isDead = false;
    }

    public void TakeDamage(Damage damage)
    {
        if (_isDead)
        {
            return;
        }

        Health -= damage.Value;
        Debug.Log(Health);
        if (Health <= 0)
        {
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, _damagedLayerMask);
        foreach (Collider collider in overlapColliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            IDamageable damagedEntity = collider.GetComponent<IDamageable>();
            if (damagedEntity == null)
            {
                continue;
            }

            Damage damage = new Damage();
            damage.Value = ExplosionDamage;
            damage.KnockBackPower = 10f;
            damage.From = this.gameObject;

            damagedEntity.TakeDamage(damage);

            string colliderTag = collider.gameObject.tag;
        }

        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        _rigidbody.AddForce(0, FlyingPower, 0, ForceMode.Impulse);
        _isDead = true;

        yield return new WaitForSeconds(LifeTime);

        gameObject.SetActive(false);
    }
}
