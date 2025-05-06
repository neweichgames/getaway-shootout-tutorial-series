using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ragdoll;

    public event Action<Player> onDeathEvent;
    public event Action<Transform> onRagdollCreateEvent;
    public event Action onRespawn;

    private int id;
    private PlayerBody body;

    private void Start()
    {
        body = GetComponent<PlayerBody>();
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

    public void TeleportPlayer(PlayerBody other)
    {
        body.SwapWithOther(other);

        // In future handle other components to be copied here (such as player zipline) by creating an onPlayerTeleport action
    }

    void Die()
    {
        // Game pool object system in future?
        PlayerBody ragdollBody = Instantiate(ragdoll, transform.position, transform.rotation).GetComponent<PlayerBody>();
        ragdollBody.SetFromOther(body);

        Destroy(ragdollBody.gameObject, 15f);

        gameObject.SetActive(false);

        onRagdollCreateEvent?.Invoke(ragdollBody.transform);
        onDeathEvent?.Invoke(this);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        body.SetBody(Vector2.zero, 0f, 0f);
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
