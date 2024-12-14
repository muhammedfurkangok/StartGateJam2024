using UnityEngine;

public class Prizma : MonoBehaviour
{
    [SerializeField] private GrabItem grabItem;
    [SerializeField] private GameObject prizmaReflectPoint;
    [SerializeField] private GameObject targetReflect;


    private void Update()
    {
        if (grabItem.GetRigidbodyVelocityMagnitude() < 0.1f && !grabItem.IsBeingGrabbed())
        {
            CheckPrizmaReflect();
        }
    }

    public void CheckPrizmaReflect()
    {
    }
}