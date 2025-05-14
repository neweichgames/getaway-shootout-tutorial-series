using UnityEngine;

/// <summary>
/// Properties for all objects in map.
/// </summary>
public class ObjectProperties : MonoBehaviour
{
    /// <summary>
    /// Determines how penetrable object is to objects such as bullets and explosions. Value of 1 being fully transparent and value of 0 being fully opaque.
    /// </summary>
    [Range(0f, 1f)]
    public float transparency;

    
    // TODO: In future add objects own particles to spawn instead of just the default particles
    /// <summary>
    /// True if object should spawn default particles when interacted with items such as bullets.
    /// </summary>
    public bool hitParticles = true;
}
