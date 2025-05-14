using System;
using UnityEngine;

/// <summary>
/// Interface for all firable bullets.
/// </summary>
public interface Bullet
{
    public event Action<Data> OnFire;

    public class Data
    {
        public Vector2 startPosition;
        public Vector2 direction;

        public LineData[] lineData;

        public Data(Vector2 startPosition, Vector2 direction, LineData[] lineData)
        {
            this.startPosition = startPosition;
            this.direction = direction;
            this.lineData = lineData;
        }
    }

    public class LineData
    {
        public float endDist;
        public float startPower;
        public float endPower;
        public Transform hitTransform;

        public LineData(float endDist, float startPower, float endPower, Transform hitTransform)
        {
            this.endDist = endDist;
            this.startPower = startPower;
            this.endPower = endPower;
            this.hitTransform = hitTransform;
        }
    }

    public void Fire(Transform shootSpot, Player owner)
    {
        Fire(shootSpot.position, shootSpot.up, owner);
    }

    public void Fire(Vector2 pos, Vector2 dir, Player owner);
}
