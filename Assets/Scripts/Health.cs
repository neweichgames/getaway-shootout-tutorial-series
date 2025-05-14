using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;

    private float curHealth;
    private bool isDead;
    private bool isInvincible;

    public event Action<DamageInfo> OnDamage;
    public event Action<DamageInfo> OnDeath;

    /// <summary>
    /// Data for damage that is applied at a point and direction.
    /// </summary>
    public class HitInfo
    {
        public Vector2 point;
        public Vector2 direction;

        public HitInfo(Vector2 point, Vector2 direction)
        {
            this.point = point;
            this.direction = direction;
        }
    }

    /// <summary>
    /// Data for all damage related events.
    /// </summary>
    public struct DamageInfo
    {
        public float damage;
        public Player user;
        public Weapon weapon;
        public HitInfo hitInfo;

        public DamageInfo(float damage, Player user, Weapon weapon) : this(damage, user, weapon, null){ }

        public DamageInfo(float damage, Player user, Weapon weapon, HitInfo hitInfo)
        {
            this.damage = damage;
            this.user = user;
            this.weapon = weapon;
            this.hitInfo = hitInfo;
        }
    }

    void Start()
    {
        curHealth = maxHealth;
    }

    public void Damage(DamageInfo info)
    {
        if (isDead || isInvincible)
            return;

        curHealth -= info.damage;
        OnDamage?.Invoke(info);

        if (curHealth <= 0)
            Die(info);
    }

    public void Die(DamageInfo info)
    {
        if(isDead) 
            return;

        curHealth = 0f;
        isDead = true;

        OnDeath?.Invoke(info);
    }

    public void SetMaxHealth()
    {
        curHealth = maxHealth;
        isDead = false;
    }

    public void IncreaseHealth(float amount, bool capHealth = true)
    {
        curHealth += amount;
        
        if(capHealth)
            curHealth = Mathf.Min(curHealth, maxHealth);
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
