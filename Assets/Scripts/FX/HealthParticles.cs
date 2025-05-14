using UnityEngine;

[RequireComponent (typeof(Health))]
public class HealthParticles : MonoBehaviour
{
    public GameObject particles;
    public Transform parent;
    public float maxHealthParticleMultiplier;
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnDamage += OnDamage;
    }

    private void OnDestroy()
    {
        health.OnDamage -= OnDamage;
    }

    private void OnDamage(Health.DamageInfo info)
    {
        if(info.hitInfo != null)
            CreateParticles(info);
    }

    void CreateParticles(Health.DamageInfo info)
    {
        Quaternion rotation = Quaternion.Euler(particles.transform.eulerAngles.x, particles.transform.eulerAngles.y, Mathf.Atan2(-info.hitInfo.direction.y, -info.hitInfo.direction.x) * Mathf.Rad2Deg);
        ParticleMultiplier p = Instantiate(particles, info.hitInfo.point, rotation, parent).GetComponentInChildren<ParticleMultiplier>();
        if (p != null)
            p.SetAmount(info.damage);
    }

    
}
