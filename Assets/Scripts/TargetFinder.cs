using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public Rigidbody2D pivot;

    private Targetable ourTarget;

    void Start()
    {
        ourTarget = GetComponent<Targetable>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Targetable target = GetTarget();

        if (target != null)
        {
            float scaleX = pivot.transform.lossyScale.x;
            Vector2 diff = (target.GetTargetPosition() - pivot.position) * scaleX;
            float targetRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            // Note: This code is currently framerate dependent. In future replace code with something that
            // isn't frame rate dependent!
            pivot.rotation = Mathf.LerpAngle(pivot.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    public void ActivateFinder(bool active)
    {
        if (active)
            pivot.constraints = RigidbodyConstraints2D.FreezeRotation;
        else
            pivot.constraints = RigidbodyConstraints2D.None;

        this.enabled = active;
    }

    Targetable GetTarget()
    {
        Targetable closestTarget = null;
        float minDistVal = Mathf.Infinity;

        foreach(Targetable target in Targetable.instances)
        {
            if (target.Equals(ourTarget))
                continue;

            Vector2 diff = target.GetTargetPosition() - (Vector2)pivot.transform.position;

            if(diff.sqrMagnitude < target.range * target.range)
            {
                float distVal = diff.sqrMagnitude / target.priority;

                if (distVal < minDistVal)
                {
                    closestTarget = target;
                    minDistVal = distVal;
                }
            }

        }

        return closestTarget;
    }
}
