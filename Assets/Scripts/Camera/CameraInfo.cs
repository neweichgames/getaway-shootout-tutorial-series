using UnityEngine;

public class CameraInfo : MonoBehaviour
{
    private int cameraID;

    public int GetCameraID()
    {
        return cameraID;
    }

    public void SetCameraID(int id)
    {
        cameraID = id;
    }
}
