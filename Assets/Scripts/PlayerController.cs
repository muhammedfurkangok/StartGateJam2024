﻿using ScriptableObjects;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin cinemachineNoise;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private NoiseSettings walkNoiseSettings;
    [SerializeField] private NoiseSettings runNoiseSettings;

    [Header("Info")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isInspecting;

    private void Update()
    {
        HandleInspection();
        HandleMouseLook();
        HandleThrowing();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInspection()
    {
        if (!PlayerGrabManager.Instance.IsHoldingItem()) return;
        if (PlayerInputManager.Instance.IsRightClickDown() && !isInspecting) isInspecting = true;
        if (PlayerInputManager.Instance.IsRightClickUp() && isInspecting) isInspecting = false;
    }

    private void HandleThrowing()
    {
        if (isInspecting) return;
        if (!PlayerGrabManager.Instance.IsHoldingItem()) return;

        if (PlayerInputManager.Instance.IsLeftClickDown())
        {
            PlayerGrabManager.Instance.GetCurrentGrabItem().ThrowItem(transform.forward);
            PlayerGrabManager.Instance.OnItemThrown();
        }
    }

    private void HandleMouseLook()
    {
        if (isInspecting) return;

        var mouseSensitivity = gameConstants.playerLookSensitivity;
        var lookDelta = PlayerInputManager.Instance.GetLookInput() * (mouseSensitivity * Time.deltaTime);

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
        if (isInspecting) return;

        var moveInput = PlayerInputManager.Instance.GetMoveInput();
        var isRunKey = PlayerInputManager.Instance.IsRunKey();

        var targetSpeed = moveInput.magnitude > 0 ? (isRunKey ? gameConstants.playerRunSpeed : gameConstants.playerWalkSpeed) : 0f;
        var acceleration = isRunKey ? gameConstants.playerRunAcceleration : gameConstants.playerWalkAcceleration;

        currentSpeed = moveInput.magnitude > 0
            ? Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime)
            : Mathf.MoveTowards(currentSpeed, 0, gameConstants.playerDeceleration * Time.fixedDeltaTime);

        var inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        var targetDirection = transform.TransformDirection(inputDirection);
        var movement = targetDirection * (currentSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(rigidbody.position + movement);

        cinemachineNoise.NoiseProfile = inputDirection.magnitude > 0
            ? isRunKey
                ? runNoiseSettings
                : walkNoiseSettings
            : null;
    }
}
