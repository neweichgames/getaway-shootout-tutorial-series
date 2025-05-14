using UnityEngine;

/// <summary>
/// Class to create explosion effects. This should most likely change to how you want the explosion to look.
/// </summary>
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
