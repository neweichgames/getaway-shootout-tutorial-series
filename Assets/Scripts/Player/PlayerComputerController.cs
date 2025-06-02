using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerComputerController : MonoBehaviour
{
    private PlayerMovement pm;
    private PlayerPowerUp pu;
    private PlayerBody body;
    private WaypointManager wm;

    private const float preciseJumpXRange = 5f;
    private const float preciseJumpYRange = 1.5f;

    private IEnumerator powerUpRoutine;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        pu = GetComponent<PlayerPowerUp>();
        body = GetComponent<PlayerBody>();

        wm = FindFirstObjectByType<WaypointManager>();

        pu.OnPowerUpCreate += OnPowerUpCreate;
        pu.OnPowerUpDeplete += OnPowerUpDeplete;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveLoop());
    }

    private void OnDestroy()
    {
        pu.OnPowerUpCreate -= OnPowerUpCreate;
        pu.OnPowerUpDeplete -= OnPowerUpDeplete;
    }

    void OnPowerUpCreate(Usable usable)
    {
        powerUpRoutine = PowerUpLoop(usable);
        StartCoroutine(powerUpRoutine);
    }

    void OnPowerUpDeplete()
    {
        StopCoroutine(powerUpRoutine);
    }

    IEnumerator PowerUpLoop(Usable usable)
    {
        int amountOfUses = 1;
        if (usable is Gun gun)
            amountOfUses = gun.startAmmo;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f / amountOfUses, 8f / amountOfUses));

            // We have power up
            pu.Use();
        }
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            if (!pm.IsGrounded())
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
                continue;
            }

            // Computer is grounded ... move to next waypoint
            yield return JumpLoop();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator JumpLoop()
    {
        Waypoint waypoint = wm.GetNavNextWaypoint(transform);
        float jumpAng = GetJumpAngle(waypoint);
        float bodyAng = body.GetRotation();
        bool turnRight = bodyAng > jumpAng;

        pm.StartTurn(turnRight);
        while ((body.GetRotation() > jumpAng) == turnRight)
            yield return new WaitForFixedUpdate();

        pm.Jump(turnRight);
    }

    float GetJumpAngle(Waypoint waypoint)
    {
        Vector2 diff = waypoint.transform.position - transform.position;

        // Check if precise jump
        if(Mathf.Abs(diff.x) < preciseJumpXRange && diff.y > preciseJumpYRange)
        {
            float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
            return Mathf.Clamp(ang, -pm.maxTurnAngle + 0.5f, pm.maxTurnAngle - 0.5f);
        }

        // Otherwise do basic jump
        int jumpDir = diff.x > 0 ? -1 : 1;
        bool jumpShort = Random.Range(0, 5) == 0;
        return jumpShort ? Random.Range(6f, 22.5f) * jumpDir : Random.Range(28f, 48f) * jumpDir;
    }
}
