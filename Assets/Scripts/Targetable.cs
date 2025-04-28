using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public static List<Targetable> instances = new List<Targetable>();

    public float range = 20f;
    public float priority = 1f;
    public Vector2 offset;

    private void OnEnable()
    {
        instances.Add(this);
    }

    private void OnDisable()
    {
        instances.Remove(this);
    }

    public Vector2 GetTargetPosition()
    {
        return transform.TransformPoint(offset);
    }
}
