using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private InputActions inputAction;

    private void Awake()
    {
        instance = this;
        inputAction = new InputActions();
        inputAction.Enable();
    }

    private void OnDestroy()
    {
        inputAction.Disable();
    }

    public bool isUpActionPressed()
    {
        return inputAction.Player.UpBinding.IsPressed();
    }

    public bool isRightActionPressed()
    {
        return inputAction.Player.RightBinding.IsPressed();
    }

    public bool isLeftActionPressed()
    {
        return inputAction.Player.LeftBinding.IsPressed();
    }

    public Vector2 GetMovementInputVector2()
    {
        return inputAction.Player.Movement.ReadValue<Vector2>();
    }
}
