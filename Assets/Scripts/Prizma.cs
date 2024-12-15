using UnityEngine;

public class Prizma : MonoBehaviour
{
    [SerializeField] private GrabItem grabItem;


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