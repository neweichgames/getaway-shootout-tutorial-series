using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gamemanager: MonoBehaviour
{
    private static GameState state;

    //TODO: change location of variables in future 
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
        if (state == null)
            state = new GameState(numPlayers);

        players = new Player[numPlayers];
        inputManager = GetComponent<PlayerInputManager>();
        // TODO: Change reference future
        waypointManager = FindFirstObjectByType<WaypointManager>();
        finish = FindFirstObjectByType<Finish>();
        finish.OnPlayersFinished += RoundOver;
        finish.OnPlayerEntered += OnPlayerFinished;

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

    void OnPlayerFinished(Player player, bool first)
    {
        state.PlayerFinished(player.GetPlayerID());
    }

    void RoundOver()
    {
        roundOver = true;
        state.RoundOver();

        foreach (Player player in players)
            player.Deactivate();

        foreach(Camera cam in cams)
            cam.gameObject.GetComponent<CameraFollow>().SetTarget(finish.transform, false);

        // Do logic of wrapping of the game ...
        if (state.GetWinner() >= 0)
            StartCoroutine(GameOverLoop());
        else
            StartCoroutine(RoundOverLoop());
    }

    IEnumerator GameOverLoop()
    {
        yield return new WaitForSeconds(1.5f);
        // Show game over screen, etc...
    }

    IEnumerator RoundOverLoop()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
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
