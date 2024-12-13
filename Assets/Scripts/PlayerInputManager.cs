using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMoveInput() => playerInputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 GetLookInput() => playerInputActions.Player.Look.ReadValue<Vector2>();
    public bool IsRunKey() => playerInputActions.Player.Run.IsInProgress();
    public bool IsInteractKeyDown() => playerInputActions.Player.Interact.WasPressedThisFrame();
}