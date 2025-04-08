using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] raycastSpots;

    [Header("Balance")]
    public float balancePower = 6.075f;
    public float maxBalancedAngle = 7.2f;

    [Header("Turn")]
    public float maxTurnSpeed = 280;
    public float lerpTurnTime = 0.5f;
    public float maxTurnAngle = 53.5f;

    [Header("Jump")]
    public float jumpPower = 11;

    bool isTurning;
    bool turnRight;
    float turningTime;
    float unbalancedTime;

    bool inContact;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current.wKey.isPressed)
            StartTurn(false);
        else if (Keyboard.current.eKey.isPressed)
            StartTurn(true);

        if (Keyboard.current.wKey.wasReleasedThisFrame)
            Jump(false);
        else if (Keyboard.current.eKey.wasReleasedThisFrame)
            Jump(true);
    }

    private void FixedUpdate()
    {
        if (isTurning)
            Turn();
        else
            Balance();   
    }

    void Balance()
    {
        float rotation = GetRotation();

        if(Mathf.Abs(rotation) > maxBalancedAngle)
        {
            if (inContact)
            {
                unbalancedTime += Time.fixedDeltaTime;
                float extraForceMult = Mathf.Clamp(Mathf.Ceil(unbalancedTime / 1.2f), 1, 4);
                float directionPower = Mathf.Clamp(-rotation, -90f, 90f);

                rb.AddTorque(directionPower * balancePower * extraForceMult);
            }
        }
        else
        {
            if(unbalancedTime > 0f)
            {
                rb.angularVelocity = rb.angularVelocity / 100f;
                unbalancedTime = 0f;
            }
        }
    }

    public void StartTurn(bool turnRight)
    {
        if (isTurning || IsGrounded() == false)
            return;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        this.turnRight = turnRight;
        isTurning = true;
    }

    public void Jump(bool jumpRight)
    {
        if (!isTurning || jumpRight != turnRight)
            return;

        if (IsGrounded())
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);

        rb.constraints = RigidbodyConstraints2D.None;
        isTurning = false;
        turningTime = 0f;
    }

    void Turn()
    {
        turningTime += Time.fixedDeltaTime;

        float rot = GetRotation();
        int dir = turnRight ? -1 : 1;

        float turnSpeed = Mathf.Min(turningTime / lerpTurnTime, 1f) * maxTurnSpeed;

        if((rot > maxTurnAngle * dir) == turnRight)
            rb.SetRotation(rot + turnSpeed * dir * Time.fixedDeltaTime);
    }

    float GetRotation()
    {
        float rot = Mathf.Repeat(rb.rotation, 360f);
        if (rot > 180f)
            rot -= 360f;
        
        return rot;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inContact = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inContact = false;
    }

    bool IsGrounded()
    {
        foreach (Transform spot in raycastSpots)
            if (Physics2D.Raycast(spot.position, -spot.up, 0.2f))
                return true;

        return false;
    }
}
