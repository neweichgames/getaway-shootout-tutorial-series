using UnityEngine;

public class Parallax : MonoBehaviour
{
    /// <summary>
    /// Amount of parallax. Value of 1 makes the object stay with the camera. Value of 0 is a normal object with no parallax.
    /// </summary>
    public float amount;
    /// <summary>
    /// Position of camera where there is no parallax. This target position is relative to the parallax object. This camera position is shown in the red frame.
    /// </summary>
    public Vector2 zeroedCamPos;
    /// <summary>
    /// Repeat size of parallax in the X and Y coordinates. If set to Vector2.Zero, there will be no repeat.
    /// </summary>
    public Vector2 repeatSize;


    private Transform cameraTransform;

    private Vector2 startPos;
    private bool isDuplicate;

    private void OnDrawGizmosSelected()
    {
        float camY = Camera.main?.orthographicSize ?? 5;
        float camX = camY * 16f / 9;
        Vector2 offset = startPos;
        if (offset.Equals(Vector2.zero))
            offset = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(offset + zeroedCamPos, new Vector3(camX * 2, camY * 2, 0.1f));
    }

    void Start()
    {
        SetStartPosition();

        if (isDuplicate == false && ParallaxManager.instance != null)
            ParallaxManager.instance.ParallaxCreated(this);
    }

    void SetStartPosition()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        UpdatePosition(GetCam(), ParallaxManager.instance?.GetParallaxOffset() ?? Vector2.zero);
    }

    void UpdatePosition(Transform cam, Vector2 offset)
    {
        Vector2 paralaxPos = ((Vector2)cam.position - (zeroedCamPos + startPos)) * amount + offset * (1 - amount);

        paralaxPos += startPos;

        float xOffset = 0f;
        if (repeatSize.x > 0)
            xOffset = Mathf.RoundToInt((cam.position.x - paralaxPos.x) / repeatSize.x) * repeatSize.x;

        float yOffset = 0f;
        if (repeatSize.x > 0)
            yOffset = Mathf.RoundToInt((cam.position.y - paralaxPos.y) / repeatSize.y) * repeatSize.y;

        transform.position = new Vector3(paralaxPos.x + xOffset, paralaxPos.y + yOffset, transform.position.z);
    }

    /// <summary>
    /// Set the camera of the parallax to track.
    /// </summary>
    /// <param name="cam">Camera to track.</param>
    public void SetCam(Transform cam)
    {
        cameraTransform = cam;
    }

    /// <summary>
    /// Get the current camera parallax is tracking.
    /// </summary>
    /// <returns>Current tracking camera. Default is main camera.</returns>
    public Transform GetCam()
    {
        if (cameraTransform == null)
            return Camera.main.transform;

        return cameraTransform.transform;
    }

    public void SetIsDuplicate(bool d = true)
    {
        isDuplicate = d;
    }

#if UNITY_EDITOR
    // Unity Editor function for manual position update ... useful for editor tools such as positioning
    // parallax for a editor screenshot
    void ForceUpdatePosition(Transform camera, Vector2 offset)
    {
        SetStartPosition();
        UpdatePosition(camera, offset);
    }
#endif
}
