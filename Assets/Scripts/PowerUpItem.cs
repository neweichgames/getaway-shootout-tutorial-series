using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpItem", menuName = "Scriptable Objects/PowerUpItem")]
public class PowerUpItem : ScriptableObject
{
    public GameObject powerUpObject;

    public Sprite icon;

    public bool findTarget;
}
