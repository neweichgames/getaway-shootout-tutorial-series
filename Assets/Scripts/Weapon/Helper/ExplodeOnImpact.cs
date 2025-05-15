using UnityEngine;

[RequireComponent (typeof(Explosive))]
public class ExplodeOnImpact : MonoBehaviour
{
    private Explosive explosive;

    void Start()
    {
        explosive = GetComponent<Explosive>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only trigger explosion on objects that are damagable
        if (!explosive.IsObjectDamagable(collision.gameObject))
            return;

        explosive.Explode();
    }
}
