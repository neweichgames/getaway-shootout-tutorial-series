using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>())
            collision.gameObject.GetComponent<Health>().Damage(60f);
    }
}
