using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int          Health = 30;
    private bool        _isDead = false;

    public GameObject   ExplosionEffectPrefab;

    public float        ExplosionRadius = 10f;
    public int          ExplosionDamage = 10;
    public LayerMask    DamagedLayerMask;

    public float        LifeTime = 3f;
    public float        FlyingPower = 100f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Start()
    {
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
        _rigidbody.AddForceAtPosition(damage.HitDirection * damage.KnockBackPower, damage.HitPoint, ForceMode.Impulse);
        if (Health <= 0)
        {
            _isDead = true;
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, DamagedLayerMask);
        foreach (Collider collider in overlapColliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            if (!collider.gameObject.activeSelf)
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
        

        yield return new WaitForSeconds(LifeTime);

        gameObject.SetActive(false);
    }
}
