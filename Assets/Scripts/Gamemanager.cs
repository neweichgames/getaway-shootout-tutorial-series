using UnityEngine;
using UnityEngine.InputSystem;

public class Gamemanager: MonoBehaviour
{
    public int numPlayers = 1;

    public GameObject playerObj;


    private PlayerInputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();

        CreateInput();
        CreatePlayers();
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
        }
    }
}
