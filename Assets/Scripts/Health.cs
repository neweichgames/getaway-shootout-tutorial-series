using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;

    private float curHealth;
    private bool isDead;
    private bool isInvincible;

    public event Action<DamageInfo> OnDamage;
    public event Action OnDeath;

    public UnityEvent OnDeathEvent;

    public class HitInfo
    {
        public Vector2 point;
        public Vector2 direction;

        public Vector2? exitPoint;

        public HitInfo(Vector2 point, Vector2 direction, Vector2? exitPoint = null)
        {
            this.point = point;
            this.direction = direction;
            this.exitPoint = exitPoint;
        }
    }

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
            Die();
    }

    public void Die()
    {
        if(isDead) 
            return;

        curHealth = 0f;
        isDead = true;

        OnDeath?.Invoke();
        OnDeathEvent?.Invoke();
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
