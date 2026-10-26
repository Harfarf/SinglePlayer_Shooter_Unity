using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum InputCommand
{
    MovementX = 0,
    MovementY = 1,
    LookX = 2,
    LookY = 3,
    Attack = 4,
    Jump = 5,
}

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private PlayerInput playerInput;
    internal bool inputEnabled = true;
    private InputAction movementInputAction;
    private InputAction lookInputAction;
    private InputAction attackInputAction;
    private InputAction jumpInputAction;



    private void OnEnable()
    {
        InitInputActions();
    }

    private void InitInputActions()
    {
        movementInputAction = playerInput.actions["Movement"];
        lookInputAction = playerInput.actions["Look"];
        attackInputAction = playerInput.actions["Attack"];
        jumpInputAction = playerInput.actions["Jump"];
    }

    public bool GetInput(InputCommand inputCommand)
    {
        if (!inputEnabled) return false;

        switch (inputCommand)
        {
            case InputCommand.Attack:
                return attackInputAction.triggered;
            case InputCommand.Jump:
                return jumpInputAction.triggered;
            default:
                return false;
        }
    }

    public float GetAxis(InputCommand inputCommand)
    {
        if (!inputEnabled) return 0;

        switch (inputCommand)
        {
            case InputCommand.MovementX:
                return movementInputAction.ReadValue<Vector2>().x;
            case InputCommand.MovementY:
                return movementInputAction.ReadValue<Vector2>().y;
            case InputCommand.LookX:
                return lookInputAction.ReadValue<Vector2>().x;
            case InputCommand.LookY:
                return lookInputAction.ReadValue<Vector2>().y;
            default:
                return 0;
        }
    }

}
