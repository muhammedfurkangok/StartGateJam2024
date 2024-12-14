using DG.Tweening;
using ScriptableObjects;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Parameters")]
    [SerializeField] private GrabItemType grabItemType;

    [Header("Info")]
    [SerializeField] private Transform target;
    [SerializeField] private bool isBeingInspected;
    [SerializeField] private bool isSnapped;
    [SerializeField] private Quaternion preInspectRotation;

    private Tween enterInspectTween;
    private Tween exitInspectTween;
    private Tween exitInspectRotationTween;
    private Tween snapTween;
    private Tween snapRotationTween;

    public GrabItemType GetGrabItemType() => grabItemType;
    public bool IsBeingGrabbed() => target != null;
    public bool GetIsBeingInspected() => isBeingInspected;

    public void SetTarget(Transform target)
    {
        this.target = target;
        rigidbody.useGravity = target == null;
        if (target != null) isSnapped = false;
    }

    public void ThrowItem(Vector3 direction)
    {
        SetTarget(null);
        rigidbody.AddForce(direction * gameConstants.grabThrowForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (target == null) return;
        if (InputManager.Instance.IsRightClickDown() && !isBeingInspected) EnterInspectMode();
        if (InputManager.Instance.IsRightClickUp() && isBeingInspected) ExitInspectMode();

        if (isBeingInspected && target != null) Inspect();
    }

    private void EnterInspectMode()
    {
        isBeingInspected = true;
        preInspectRotation = transform.rotation;
        rigidbody.Sleep();

        enterInspectTween?.Kill();
        exitInspectTween?.Kill();
        exitInspectRotationTween?.Kill();
        snapTween?.Kill();
        snapRotationTween?.Kill();

        enterInspectTween = transform.DOMove(PlayerGrabManager.Instance.GetInspectPosition().position, gameConstants.grabItemEnterInspectDuration)
            .SetEase(gameConstants.grabItemEnterInspectEase);
    }

    private void ExitInspectMode()
    {
        isBeingInspected = false;
        rigidbody.WakeUp();

        enterInspectTween?.Kill();
        exitInspectTween?.Kill();
        exitInspectRotationTween?.Kill();
        snapTween?.Kill();
        snapRotationTween?.Kill();

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

        var mouseX = InputManager.Instance.GetLookInput().x * gameConstants.playerInspectSensitivity * Time.deltaTime;
        var mouseY = InputManager.Instance.GetLookInput().y * gameConstants.playerInspectSensitivity * Time.deltaTime;

        var rotationX = Quaternion.AngleAxis(mouseX, Vector3.up);
        var rotationY = Quaternion.AngleAxis(-mouseY, Vector3.right);

        transform.rotation = rotationX * rotationY * transform.rotation;
    }

    private void FixedUpdate()
    {
        if (isBeingInspected) return;

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

    public void TrySnap(Vector3 snapPosition)
    {
        if (isSnapped) return;
        if (IsBeingGrabbed()) return;
        if (rigidbody.linearVelocity.magnitude > gameConstants.grabItemSnapMaxVelocity) return;

        isSnapped = true;

        snapTween?.Kill();
        snapRotationTween?.Kill();

        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        snapTween = transform.DOMove(snapPosition, gameConstants.grabItemSnapDuration)
            .SetEase(gameConstants.grabItemSnapEase);

        snapRotationTween = transform.DORotateQuaternion(Quaternion.identity, gameConstants.grabItemSnapDuration)
            .SetEase(gameConstants.grabItemSnapRotationEase);
    }
}