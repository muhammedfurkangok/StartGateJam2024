using System;
using ScriptableObjects;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Input")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
    [SerializeField] private bool isInteractKeyDown;
    [SerializeField] private bool isRunKey;

    [Header("Movement")]
    [SerializeField] private float currentSpeed;

    private PlayerInputActions playerInputActions;

    private void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Enable();
    }

    private void Update()
    {
        GetInput();
        HandleMouseLook();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void GetInput()
    {
        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        lookInput = playerInputActions.Player.Look.ReadValue<Vector2>();
        isRunKey = playerInputActions.Player.Run.IsPressed();
    }

    private void HandleMouseLook()
    {
        var mouseSensitivity = gameConstants.playerLookSensitivity;
        var lookDelta = lookInput * (mouseSensitivity * Time.deltaTime);

        transform.Rotate(Vector3.up, lookDelta.x);

        var cameraLocalEulerAngles = cinemachineCamera.transform.localEulerAngles;

        var newCameraYAngle = cameraLocalEulerAngles.y + lookDelta.x;

        var newCameraXAngle = cameraLocalEulerAngles.x - lookDelta.y;
        if (newCameraXAngle > 180) newCameraXAngle -= 360;
        newCameraXAngle = Mathf.Clamp(newCameraXAngle, -90, 90);

        cameraLocalEulerAngles.x = newCameraXAngle;
        cameraLocalEulerAngles.y = newCameraYAngle;
        cinemachineCamera.transform.localEulerAngles = cameraLocalEulerAngles;
    }
    
    private void HandleMovement()
    {
        var targetSpeed = moveInput.magnitude > 0 ? (isRunKey ? gameConstants.playerRunSpeed : gameConstants.playerWalkSpeed) : 0f;
        var inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        var targetDirection = transform.TransformDirection(inputDirection);

        currentSpeed = moveInput.magnitude > 0
            ? Mathf.MoveTowards(currentSpeed, targetSpeed, gameConstants.playerAcceleration * Time.fixedDeltaTime)
            : Mathf.MoveTowards(currentSpeed, 0, gameConstants.playerDeceleration * Time.fixedDeltaTime);

        var currentVelocity = targetDirection * currentSpeed;
        var movement = currentVelocity * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }
}
