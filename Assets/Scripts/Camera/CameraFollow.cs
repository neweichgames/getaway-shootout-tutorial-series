using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float lerpSpeed = 5f;
    public Vector2 offset;

    private Transform target;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (target != null)
            Follow();
    }


    void Follow()
    {
        Vector3 targetPos = GetTargetPos();

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
    }

    public void SetTarget(Transform target, bool snapToTarget)
    {
        this.target = target;
        if (snapToTarget)
            transform.position = GetTargetPos();
    }

    Vector3 GetTargetPos()
    {
        return new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
    }
}
