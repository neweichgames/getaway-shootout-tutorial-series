using UnityEngine;

public class GameCameraController : MonoBehaviour
{

    CameraFollow cf;
    Player player;

    void Start()
    {
        cf = GetComponent<CameraFollow>();

        int playerID = GetComponent<CameraInfo>().GetCameraID();
        player = FindFirstObjectByType<Gamemanager>().GetPlayer(playerID);

        cf.SetTarget(player.transform, true);

        player.onRagdollCreateEvent += OnPlayerRagdoll;
        player.onRespawn += OnPlayerRespawn;
    }

    private void OnDisable()
    {
        player.onRagdollCreateEvent -= OnPlayerRagdoll;
        player.onRespawn -= OnPlayerRespawn;
    }


    void OnPlayerRagdoll(Transform ragdoll)
    {
        cf.SetTarget(ragdoll, false);
    }

    void OnPlayerRespawn()
    {
        cf.SetTarget(player.transform, false);
    }
}
