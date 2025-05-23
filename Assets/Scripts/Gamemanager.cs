using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gamemanager: MonoBehaviour
{
    public int numPlayers = 1;

    public GameObject playerObj;
    public CameraManager cameraManager;

    private Player[] players;
    private PlayerInputManager inputManager;

    private void Awake()
    {
        players = new Player[numPlayers];
        inputManager = GetComponent<PlayerInputManager>();

        CreateInput();
        CreatePlayers();
        CreateCameras();
    }


    void CreateInput()
    {
        for (int i = 0; i < numPlayers; i++)
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
            player.AddComponent<PlayerHumanController>();
            players[i] = player.GetComponent<Player>();

            players[i].onDeathEvent += OnPlayerDeath;
        }
    }

    void CreateCameras()
    {
        Camera[] cameras = cameraManager.Initalize(numPlayers);
        GetComponent<ParallaxManager>().cameras = cameras;
    }

    public Player GetPlayer(int playerID)
    {
        return players[playerID];
    }

    void OnPlayerDeath(Player player)
    {
        StartCoroutine(RespawnPlayerLoop(player));
    }

    IEnumerator RespawnPlayerLoop(Player player)
    {
        yield return new WaitForSeconds(5f);
        
        // respawn player
        player.Respawn();
        player.transform.position = Vector3.zero;
    }

    private void OnDisable()
    {
        for(int i = 0; i < players.Length; i++)
            players[i].onDeathEvent -= OnPlayerDeath;
    }
}
