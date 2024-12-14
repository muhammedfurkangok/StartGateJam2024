using ScriptableObjects;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Info")]
    [SerializeField] private Transform target;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;

        rigidbody.useGravity = target == null;
    }

    private void FixedUpdate()
    {
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