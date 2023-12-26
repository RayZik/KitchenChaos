using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            string savedJson = PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS);
            inputActions.LoadBindingOverridesFromJson(savedJson);
        }

        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        return binding switch
        {
            Binding.InteractAlternate => inputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            Binding.Pause => inputActions.Player.Pause.bindings[0].ToDisplayString(),
            Binding.Move_Up => inputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.Move_Down => inputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.Move_left => inputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.Move_Right => inputActions.Player.Move.bindings[4].ToDisplayString(),
            Binding.Interact => inputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.Gamepad_Interact => inputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.Gamepad_InteractAlternate => inputActions.Player.InteractAlternate.bindings[1].ToDisplayString(),
            Binding.Gamepad_Pause => inputActions.Player.Pause.bindings[1].ToDisplayString(),
            _ => throw new NotImplementedException(),
        };
    }

    public void RebindBinding(Binding binding, Action onActionRebind)
    {
        inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_left:
                inputAction = inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = inputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.InteractAlternate:
                inputAction = inputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = inputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction
            .PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                onActionRebind();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
