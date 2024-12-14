using ScriptableObjects;
using UnityEngine;

public class GrabItemPosition : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private GrabItemType neededGrabItemType;

    [Header("Info")]
    [SerializeField] private bool isCompleted;
    [SerializeField] private GrabItem currentGrabItem;

    public bool IsCompleted() => isCompleted;
    public void GrabItemOnly_SetIsCompleted(bool value) => isCompleted = value;

    private void OnTriggerStay(Collider other)
    {
        if (isCompleted) return;
        if (!other.CompareTag("GrabItem")) return;

        var grabItem = other.GetComponent<GrabItem>();
        currentGrabItem = grabItem;

        grabItem.TrySnap(transform.position);
    }
}