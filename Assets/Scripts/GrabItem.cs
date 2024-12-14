using ScriptableObjects;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Info")]
    [SerializeField] private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;

        if (target == null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.useGravity = true;
        }

        else
        {
            rigidbody.useGravity = false;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        var direction = target.position - transform.position;
        var distance = direction.magnitude;

        direction.Normalize();
        var desiredVelocity = direction * (gameConstants.grabForce * distance);

        rigidbody.linearVelocity = desiredVelocity;
    }
}