using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyTime = 1f;
    
    void Start()
    {
        Destroy(gameObject, destroyTime);    
    }
}
