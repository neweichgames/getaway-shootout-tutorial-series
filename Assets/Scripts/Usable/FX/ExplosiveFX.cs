using UnityEngine;

[RequireComponent(typeof(Explosive))]
public class ExplosiveFX : MonoBehaviour
{
    public GameObject explosionParticles;
    public bool destroyObject = true;

    void Start()
    {
        GetComponent<Explosive>().OnExplode += CreateEffects;
    }

    void CreateEffects()
    {
        GameObject e = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(e, 5f);
        
        if (destroyObject)
            Destroy(gameObject);
    }
}
