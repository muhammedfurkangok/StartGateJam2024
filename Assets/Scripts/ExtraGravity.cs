using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rigidbody;

    [Header("Info")]
    [SerializeField] private float extraGravity;

    private void FixedUpdate()
    {
        if (rigidbody.useGravity) rigidbody.AddForce(Vector3.down * extraGravity);
    }
}