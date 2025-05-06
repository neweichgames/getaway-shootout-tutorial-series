using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpItem", menuName = "Scriptable Objects/PowerUpItem")]
public class PowerUpItem : ScriptableObject
{
    public GameObject powerUpObject;

    public Sprite icon;

    public bool findTarget;

    /// <summary>
    /// The amount of time to destory power up after Usable object has signaled depleted.
    /// </summary>
    public float destroyDelay = 0.5f;
}
