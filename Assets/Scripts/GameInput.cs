using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private InputActions inputAction;

    public event EventHandler OnMenuButtonPressed;

    private void Awake()
    {
        instance = this;
        inputAction = new InputActions();
        inputAction.Enable();

        inputAction.Player.Menu.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
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
