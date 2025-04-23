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
    public float jumpCoolDown = 0.15f;

    bool isTurning;
    bool turnRight = true;
    float turningTime;
    float unbalancedTime;
    float jumpTime;

    bool inContact;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(jumpTime >= 0f)
            jumpTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isTurning)
            Turn();
        else
            Balance();

        // Reset our inContact variable
        // Note: inContact will be a frame behind since OnCollisionStay2D runs after fixed update
        // this should be fine in our case
        inContact = false;
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
        if (isTurning || IsGrounded() == false || jumpTime > 0f)
            return;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Flip character to turn direction
        // TODO: Most likely control by a separate script and listen to OnTurnDirChange
        if (this.turnRight != turnRight)
        {
            transform.GetChild(0).localScale = new Vector3(turnRight ? 1 : -1, 1, 1);
            transform.GetChild(0).GetChild(1).eulerAngles = new Vector3(0, 0, transform.GetChild(0).GetChild(1).eulerAngles.z * -1f);
        }

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        inContact = true;
    }

    bool IsGrounded()
    {
        foreach (Transform spot in raycastSpots)
            if (Physics2D.Raycast(spot.position, -spot.up, 0.2f))
                return true;

        return false;
    }

    private void OnDisable()
    {
        isTurning = false;
        turningTime = 0f;
        unbalancedTime = 0f;
        jumpTime = 0f;

        rb.constraints = RigidbodyConstraints2D.None;

        inContact = false;
    }
}
