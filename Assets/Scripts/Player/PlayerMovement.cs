using UnityEngine;

/// <summary>
/// Class for controlling player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public Transform[] raycastSpots;

    [Header("Balance")]
    /// <summary>
    /// Force to apply when balancing player.
    /// </summary>
    public float balanceForce = 0.3f;
    /// <summary>
    /// Maximum absolute angle of player in balance. Outside this angle player will actively try to balance.
    /// </summary>
    public float maxBalancedAngle = 7.2f;

    [Header("Turn")]
    /// <summary>
    /// Maximum turn speed to rotate player when turning.
    /// </summary>
    public float maxTurnSpeed = 280;
    /// <summary>
    /// Time it takes to reach maximum turn speed.
    /// </summary>
    public float lerpTurnTime = 0.5f;
    /// <summary>
    /// Maximum angle that player can turn before stopping.
    /// </summary>
    public float maxTurnAngle = 53.5f;

    [Header("Jump")]
    /// <summary>
    /// Force applied when jumping.
    /// </summary>
    public float jumpForce = 11;
    /// <summary>
    /// Cool down time before player can jump again.
    /// </summary>
    public float jumpCoolDown = 0.15f;

    bool isTurning;
    bool turnRight = true;
    float turningTime;
    float unbalancedTime;
    float jumpTime;

    bool inContact;

    Rigidbody2D rb;
    PlayerBody body;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        body = GetComponent<PlayerBody>();
    }

    private void Update()
    {
        if(jumpTime >= 0f)
            jumpTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Turn body if turning ... otherwise default to always balancing the player
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
        float rotation = body.GetRotation();

        // Check if we are outside our maximum balanced zone ... if so and if in contact, apply force to make player upright
        if(Mathf.Abs(rotation) > maxBalancedAngle)
        {
            if (inContact)
            {
                unbalancedTime += Time.fixedDeltaTime;
                float extraForceMult = Mathf.Clamp(Mathf.Ceil(unbalancedTime / 1.2f), 1, 4);
                float directionPower = Mathf.Clamp(-rotation, -90f, 90f);

                rb.AddTorque(directionPower * balanceForce * extraForceMult);
            }
        }
        else
        {
            // Check if this is the first time entering the balanced zone ... if so, manually decrease the angular velocity
            if(unbalancedTime > 0f)
            {
                rb.angularVelocity = rb.angularVelocity / 100f;
                unbalancedTime = 0f;
            }
        }
    }

    /// <summary>
    /// Start player turning in a direction.
    /// </summary>
    /// <param name="turnRight">Turn direction.</param>
    public void StartTurn(bool turnRight)
    {
        if (isTurning || IsGrounded() == false || jumpTime > 0f)
            return;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        body.SetDirection(turnRight);

        this.turnRight = turnRight;
        isTurning = true;
    }

    /// <summary>
    /// Jump player in a direction. Jump direction is used to make sure it matches turn direction.
    /// </summary>
    /// <param name="jumpRight">Jump direction.</param>
    public void Jump(bool jumpRight)
    {
        if (!isTurning || jumpRight != turnRight)
            return;

        // TODO: Jumping force can be changed in the future to prevent player from jumping with too much velocity.
        // Possiblities are setting rigidbody velocity manually instead of AddForce
        // or making a check to make sure the mangitude of velocity is less than some value in the direction of player jumping.
        // Only adding jumping force if grounded
        if (IsGrounded())
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        rb.constraints = RigidbodyConstraints2D.None;
        isTurning = false;
        turningTime = 0f;
    }

    void Turn()
    {
        turningTime += Time.fixedDeltaTime;

        float rot = body.GetRotation();
        int dir = turnRight ? -1 : 1;

        // Increasing turn speed like this may have frame rate dependent turning speed. There may be other solutions to avoid this.
        float turnSpeed = Mathf.Min(turningTime / lerpTurnTime, 1f) * maxTurnSpeed;

        // Check if we are inside the max turn angle ... if so, continue to increase our rotation
        if((rot > maxTurnAngle * dir) == turnRight)
            rb.SetRotation(rot + turnSpeed * dir * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        inContact = true;
    }

    bool IsGrounded()
    {
        // Check if grounded by raycasting for collision at all defined raycast spots
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
