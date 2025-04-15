using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHumanController : MonoBehaviour
{

    PlayerMovement pm;
    PlayerInput input;

    InputAction moveLeft;
    InputAction moveRight;
    InputAction powerUp;

    bool isLeftDown;
    bool isRightDown;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();

        input = PlayerInput.GetPlayerByIndex(GetComponent<Player>().GetPlayerID());
        Debug.Log(GetComponent<Player>().GetPlayerID());

        moveLeft = input.currentActionMap.FindAction("MoveLeft");
        moveRight = input.currentActionMap.FindAction("MoveRight");
        powerUp = input.currentActionMap.FindAction("PowerUp");
    }

    private void OnEnable()
    {
        moveLeft.started += OnMoveCallback;
        moveLeft.canceled += OnMoveCallback;
        moveRight.started += OnMoveCallback;
        moveRight.canceled += OnMoveCallback;
    }

    private void OnDisable()
    {
        moveLeft.started -= OnMoveCallback;
        moveLeft.canceled -= OnMoveCallback;
        moveRight.started -= OnMoveCallback;
        moveRight.canceled -= OnMoveCallback;

        isLeftDown = false;
        isRightDown = false;
    }

    private void Update()
    {
        if (isLeftDown)
            pm.StartTurn(false);
        else if(isRightDown)
            pm.StartTurn(true);
    }

    void OnMoveCallback(InputAction.CallbackContext context)
    {
        bool isRight = context.action.Equals(moveRight);
        
        if (context.started)
        {
            if (isRight)
                isRightDown = true;
            else
                isLeftDown = true;
        }
           
        else if (context.canceled)
        {
            pm.Jump(isRight);
            if (isRight)
                isRightDown = false;
            else
                isLeftDown = false;
        }       
    }
}
