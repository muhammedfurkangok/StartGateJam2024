using System;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Info")]
    [SerializeField] private bool IsBeingInspected;
    [SerializeField] private Transform target;
    [SerializeField] private Quaternion preInspectRotation;

    private Tween enterInspectTween;
    private Tween exitInspectTween;
    private Tween exitInspectRotationTween;

    public void SetTarget(Transform target)
    {
        this.target = target;
        rigidbody.useGravity = target == null;
    }

    public void ThrowItem(Vector3 direction)
    {
        SetTarget(null);
        rigidbody.AddForce(direction * gameConstants.grabThrowForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (target == null) return;
        if (PlayerInputManager.Instance.IsRightClickDown() && !IsBeingInspected) EnterInspectMode();
        if (PlayerInputManager.Instance.IsRightClickUp() && IsBeingInspected) ExitInspectMode();

        if (IsBeingInspected && target != null) Inspect();
    }

    private void EnterInspectMode()
    {
        IsBeingInspected = true;
        preInspectRotation = transform.rotation;
        rigidbody.Sleep();

        enterInspectTween?.Kill();
        exitInspectTween?.Kill();
        exitInspectRotationTween?.Kill();

        enterInspectTween = transform.DOMove(PlayerGrabManager.Instance.GetInspectPosition().position, gameConstants.grabItemEnterInspectDuration)
            .SetEase(gameConstants.grabItemEnterInspectEase);
    }

    private void ExitInspectMode()
    {
        IsBeingInspected = false;
        rigidbody.WakeUp();

        enterInspectTween?.Kill();
        exitInspectTween?.Kill();
        exitInspectRotationTween?.Kill();

        exitInspectTween = transform.DOMove(PlayerGrabManager.Instance.GetHoldPosition().position, gameConstants.grabItemExitInspectDuration)
            .SetEase(gameConstants.grabItemExitInspectEase);

        exitInspectRotationTween = transform.DORotateQuaternion(preInspectRotation, gameConstants.grabItemExitInspectDuration)
            .SetEase(gameConstants.grabItemExitInspectRotationEase);
    }

    private void Inspect()
    {
        if (enterInspectTween != null && enterInspectTween.IsActive()) return;
        if (exitInspectTween != null && exitInspectTween.IsActive()) return;
        if (exitInspectRotationTween != null && exitInspectRotationTween.IsActive()) return;

        var mouseX = PlayerInputManager.Instance.GetLookInput().x * gameConstants.playerInspectSensitivity * Time.deltaTime;
        var mouseY = PlayerInputManager.Instance.GetLookInput().y * gameConstants.playerInspectSensitivity * Time.deltaTime;

        var rotationX = Quaternion.AngleAxis(mouseX, Vector3.up);
        var rotationY = Quaternion.AngleAxis(-mouseY, Vector3.right);

        transform.rotation = rotationX * rotationY * transform.rotation;
    }

    private void FixedUpdate()
    {
        if (IsBeingInspected) return;

        if (target == null)
        {
            rigidbody.linearVelocity -= rigidbody.linearVelocity.normalized * (gameConstants.grabReleaseDeceleration * Time.fixedDeltaTime);
            return;
        }

        var direction = target.position - transform.position;
        var distance = direction.magnitude;
        direction.Normalize();

        var desiredLinearVelocity = direction * (gameConstants.grabForce * distance);
        rigidbody.linearVelocity = desiredLinearVelocity;

        var torqueAxis = Vector3.Cross(Vector3.up, direction).normalized;
        var angularMagnitude = desiredLinearVelocity.magnitude * gameConstants.grabAngularForce;
        rigidbody.angularVelocity = torqueAxis * angularMagnitude;
    }
}