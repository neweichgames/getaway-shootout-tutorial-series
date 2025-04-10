using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager instance;

    public Camera[] cameras;
    public int baseCameraParallaxLayer = 14;

    private Vector2 offset;
    private bool hasInitCameraLayers;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogError("Duplicate instance created!");
            }

            return;
        }

        instance = this;
    }

    public void ParallaxCreated(Parallax p)
    {
        if (cameras.Length == 0)
            return;

        // Set root parallax to camera 0
        p.SetCam(cameras[0].transform);

        if (cameras.Length > 1)
        {
            if (hasInitCameraLayers == false)
                InitCameraLayers();

            UpdateParallaxLayer(p, 0);
        }

        // Loop through all other cameras and create duplicate parallax layer
        for (int i = 1; i < cameras.Length; i++)
        {
            Parallax duplicate = Instantiate(p.gameObject, p.transform.position, p.transform.rotation, p.transform.parent).GetComponent<Parallax>();
            duplicate.SetIsDuplicate(true);
            duplicate.SetCam(cameras[i].transform);
            UpdateParallaxLayer(duplicate, i);
        }
    }

    void InitCameraLayers()
    {
        int mask = ~(((1 << cameras.Length) - 1) << baseCameraParallaxLayer);
        int curCamMask = 1 << baseCameraParallaxLayer;
        for (int i = 0; i < cameras.Length; i++)
        {
            mask += curCamMask;
            cameras[i].cullingMask &= mask;

            if (i != 0)
                curCamMask <<= 1;
        }

        hasInitCameraLayers = true;
    }

    void UpdateParallaxLayer(Parallax p, int layerIndex)
    {
        foreach (Transform child in p.GetComponentsInChildren<Transform>())
            child.gameObject.layer = baseCameraParallaxLayer + layerIndex;
    }

    public void SetParallaxOffset(Vector2 offset)
    {
        this.offset = offset;
    }

    public Vector2 GetParallaxOffset()
    {
        return offset;
    }

    void OnDestroy()
    {
        instance = null;
    }
}