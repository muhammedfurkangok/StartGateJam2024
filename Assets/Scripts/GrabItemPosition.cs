using ScriptableObjects;
using UnityEngine;

public class GrabItemPosition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Parameters")]
    [SerializeField] private GrabItemType neededGrabItemType;

    [Header("Info")]
    [SerializeField] private bool isCompleted;
    [SerializeField] private GrabItem currentGrabItem;

    public bool IsCompleted() => isCompleted;
    public GrabItem GetCurrentGrabItem() => currentGrabItem;
    public GrabItemType GetNeededGrabItemType() => neededGrabItemType;
    public void SetCompleted()
    {
        isCompleted = true;
        meshRenderer.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCompleted) return;
        if (!other.CompareTag("GrabItem")) return;

        var grabItem = other.GetComponent<GrabItem>();
        currentGrabItem = grabItem;

        grabItem.TrySnap(this);
    }
}