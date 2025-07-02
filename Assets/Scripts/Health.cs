using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;

    private float _curHealth;
    private bool isDead;
    private bool isInvincible;

    public event Action<DamageInfo> OnDamage;
    public event Action<DamageInfo> OnDeath;
    public event Action<float> OnHealthChanged;

    public float CurHealth
    {
        get => _curHealth;
        set
        {
            if(_curHealth != value)
            {
                _curHealth = value;
                OnHealthChanged?.Invoke(value);
            }
        }
    }

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

    void Awake()
    {
        _curHealth = maxHealth;
    }

    public void Damage(DamageInfo info)
    {
        if (isDead || isInvincible)
            return;

        CurHealth -= info.damage;
        OnDamage?.Invoke(info);

        if (CurHealth <= 0)
            Die(info);
    }

    public void Die(DamageInfo info)
    {
        if(isDead) 
            return;

        CurHealth = 0f;
        isDead = true;

        OnDeath?.Invoke(info);
    }

    public void SetMaxHealth()
    {
        CurHealth = maxHealth;
        isDead = false;
    }

    public void IncreaseHealth(float amount, bool capHealth = true)
    {
        CurHealth = capHealth ? Mathf.Min(CurHealth + amount, maxHealth) : CurHealth + amount;
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
