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
    public ScoreboardUI scoreboardUI;

    private Player[] players;
    private Camera[] cams;
    private PlayerInputManager inputManager;
    private WaypointManager waypointManager;
    private MapManager mapManager;
    private Finish finish;
    
    private bool roundOver;

    private void Awake()
    {
        if (state == null)
            state = new GameState(numPlayers);

        players = new Player[numPlayers];
        inputManager = GetComponent<PlayerInputManager>();
        mapManager = GetComponent<MapManager>();

        scoreboardUI.Init(state.GetScores());
        state.OnScoreChanged += scoreboardUI.UpdateScores;

        CreateInput();
        CreateMap();
        CreatePlayers();
        CreateCameras();
    }

    private void OnDestroy()
    {
        state.OnScoreChanged -= scoreboardUI.UpdateScores;
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

    void CreateMap()
    {
        mapManager.CreateMap(state);

        // TODO: Change reference future
        waypointManager = FindFirstObjectByType<WaypointManager>();
        if (waypointManager == null)
            throw new System.Exception("Map does not contain Waypoint Manager! Add waypoint manager to map for navigation and spawn points.");
        finish = FindFirstObjectByType<Finish>();
        if (finish == null)
            Debug.LogError("Map does not contain finish! Add finish object for players to complete the map.");
        else
        {
            finish.OnPlayersFinished += RoundOver;
            finish.OnPlayerEntered += OnPlayerFinished;
        }
    }

    void CreatePlayers()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            int playerID = state.playerOrder[i];

            GameObject player = Instantiate(playerObj);
            player.GetComponent<Player>().SetPlayerID(playerID);
            if (playerID < numHumanPlayers)
                player.AddComponent<PlayerHumanController>();
            else
                player.AddComponent<PlayerComputerController>();
            players[playerID] = player.GetComponent<Player>();

            players[playerID].onDeathEvent += OnPlayerDeath;
            waypointManager.AddNav(player.transform);

            Vector2 spawnPos = waypointManager.GetNavCurWaypoint(player.transform).transform.position;
            spawnPos += Vector2.right * 0.75f * i;
            player.GetComponent<PlayerBody>().SetBody(spawnPos, 0f, 0f);
        }
    }

    void CreateCameras()
    {
        cams = cameraManager.Initalize(numHumanPlayers);
        GetComponent<ParallaxManager>().cameras = cams;

        for (int i = 0; i < cams.Length; i++)
        {
            Health h = players[i].GetComponent<Health>();
            cams[i].GetComponentInChildren<HealthUI>().Init(h);

            PlayerPowerUp pu = players[i].GetComponent<PlayerPowerUp>();
            cams[i].GetComponentInChildren<PowerUpUI>().Init(pu);
        }
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
        state.RoundOver(GetPlayerOrder());

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

    int[] GetPlayerOrder()
    {
        Transform[] navs = waypointManager.GetNavDistanceOrder();
        int[] ids = new int[navs.Length];
        for (int i = 0; i < navs.Length; i++)
            ids[i] = navs[i].GetComponent<Player>().GetPlayerID();
        
        return ids;
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
