using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    
    void Start()
    {
        int playerID = GetComponent<CameraInfo>().GetCameraID();
        Transform playerObj = FindFirstObjectByType<Gamemanager>().GetPlayer(playerID).transform;
        GetComponent<CameraFollow>().SetTarget(playerObj, true);
    }
}
