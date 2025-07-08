
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance {  get; private set; }

    public event EventHandler OnInterAction;
    public event EventHandler OnInteractAlterion;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Intercat,
        InteractAlternate,
        Pause,

    }

    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        playerInputAction.Player.Enable();

        playerInputAction.Player.Intercat.performed += Intercat_performed1;
        playerInputAction.Player.IntercatAlternate.performed += IntercatAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy()
    {
        playerInputAction.Player.Intercat.performed -= Intercat_performed1;
        playerInputAction.Player.IntercatAlternate.performed -= IntercatAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }

    private void IntercatAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlterion?.Invoke(this,EventArgs.Empty);
    }

    private void Intercat_performed1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //throw new System.NotImplementedException();
        OnInterAction?.Invoke(this,EventArgs.Empty);

    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

         inputVector = inputVector.normalized;

        return inputVector;
    }


    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Intercat:
                return playerInputAction.Player.Intercat.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAction.Player.IntercatAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
        }
    }
    public void Rebinding(Binding binding,Action onActionRebound)
    {
        playerInputAction.Player.Disable();

        InputAction inputAction;

        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Intercat:
                inputAction = playerInputAction.Player.Intercat;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputAction.Player.IntercatAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputAction.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this,EventArgs.Empty);
            }).Start();
    }
}
  