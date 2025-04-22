using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ragdoll;

    public event Action<Player> onDeathEvent;
    public event Action<Transform> onRagdollCreateEvent;
    public event Action onRespawn;

    private int id;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Health>().OnDeath += Die;
    }

    public void OnDestroy()
    {
        GetComponent<Health>().OnDeath -= Die;
    }

    public int GetPlayerID()
    {
        return id;
    }

    public void SetPlayerID(int id)
    {
        this.id = id;
    }

    void Die()
    {
        // Game pool object system in future?
        GameObject r = Instantiate(ragdoll, transform.position, transform.rotation);
        
        r.GetComponent<Rigidbody2D>().linearVelocity = rb.linearVelocity;
        r.GetComponent<Rigidbody2D>().angularVelocity = rb.angularVelocity;

        float armVel = transform.GetChild(0).GetChild(1).GetComponent<Rigidbody2D>().angularVelocity;
        r.transform.GetChild(0).GetChild(1).GetComponent<Rigidbody2D>().angularVelocity = armVel;

        Transform holdSpot = transform.GetChild(0).GetChild(1).GetChild(1);
        Transform ragdollHoldSpot = r.transform.GetChild(0).GetChild(1).GetChild(1);

        foreach (Transform child in holdSpot)
        {
            Vector3 pos = child.localPosition;
            Quaternion rot = child.localRotation;

            child.parent = ragdollHoldSpot;
            child.localPosition = pos;
            child.localRotation = rot;
        }
            

        Destroy(r, 15f);

        gameObject.SetActive(false);

        onRagdollCreateEvent?.Invoke(r.transform);
        onDeathEvent?.Invoke(this);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Health>().SetMaxHealth();
        onRespawn?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Death")
        {
            Die();
        }
    }
}
