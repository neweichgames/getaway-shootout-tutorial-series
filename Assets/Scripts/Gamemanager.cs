using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gamemanager: MonoBehaviour
{
    public int numPlayers = 1;
    public int numHumanPlayers = 1;

    public GameObject playerObj;
    public CameraManager cameraManager;

    private Player[] players;
    private Camera[] cams;
    private PlayerInputManager inputManager;
    private WaypointManager waypointManager;
    private Finish finish;

    private bool roundOver;

    private void Awake()
    {
        players = new Player[numPlayers];
        inputManager = GetComponent<PlayerInputManager>();
        // TODO: Change reference future
        waypointManager = FindFirstObjectByType<WaypointManager>();
        finish = FindFirstObjectByType<Finish>();
        finish.OnPlayersFinished += OnPlayersFinished;

        CreateInput();
        CreatePlayers();
        CreateCameras();
    }


    void CreateInput()
    {
        for (int i = 0; i < numHumanPlayers; i++)
        {
            if (i == 0)
                inputManager.JoinPlayer(playerIndex: i, controlScheme: "KeyboardLeft", pairWithDevices: Keyboard.current);
            else if (i == 1)
                inputManager.JoinPlayer(playerIndex: i, controlScheme: "KeyboardRight", pairWithDevices: Keyboard.current);
            else
                inputManager.JoinPlayer(playerIndex: i, controlScheme: "Gamepad");
        }   
    }

    void CreatePlayers()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = Instantiate(playerObj);
            player.GetComponent<Player>().SetPlayerID(i);
            if (i < numHumanPlayers)
                player.AddComponent<PlayerHumanController>();
            else
                player.AddComponent<PlayerComputerController>();
            players[i] = player.GetComponent<Player>();

            players[i].onDeathEvent += OnPlayerDeath;
            waypointManager.AddNav(player.transform);

            Vector2 spawnPos = waypointManager.GetNavCurWaypoint(player.transform).transform.position;
            player.GetComponent<PlayerBody>().SetBody(spawnPos, 0f, 0f);
        }
    }

    void CreateCameras()
    {
        cams = cameraManager.Initalize(numHumanPlayers);
        GetComponent<ParallaxManager>().cameras = cams;
    }

    public Player GetPlayer(int playerID)
    {
        return players[playerID];
    }

    void OnPlayerDeath(Player player)
    {
        StartCoroutine(RespawnPlayerLoop(player));
    }

    void OnPlayersFinished(Player[] finishedPlayers)
    {
        roundOver = true;

        foreach (Player player in players)
            player.Deactivate();

        foreach(Camera cam in cams)
            cam.gameObject.GetComponent<CameraFollow>().SetTarget(finish.transform, false);

        // Do logic of wrapping of the game ...
    }

    IEnumerator RespawnPlayerLoop(Player player)
    {
        yield return new WaitForSeconds(1.5f);

        if (roundOver)
            yield break;

        // respawn player
        player.Respawn();
        Vector2 spawnPos = waypointManager.GetNavSpawnWaypoint(player.transform).transform.position;
        player.GetComponent<PlayerBody>().SetBody(spawnPos, 0f, 0f);
    }

    private void OnDisable()
    {
        for(int i = 0; i < players.Length; i++)
            players[i].onDeathEvent -= OnPlayerDeath;
    }
}
