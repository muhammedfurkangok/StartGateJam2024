using System;
using UnityEngine;

public class DoorCompletionCheckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GrabItem[] firstDoorItems;
    [SerializeField] private GrabItem[] secondDoorItems;
    [SerializeField] private GrabItem[] thirdDoorItems;
    [SerializeField] private Transform firstDoorParticle;
    [SerializeField] private Transform secondDoorParticle;
    [SerializeField] private Transform thirdDoorParticle;

    [Header("Info")]
    [SerializeField] private bool firstDoorCompleted;
    [SerializeField] private bool secondDoorCompleted;
    [SerializeField] private bool thirdDoorCompleted;

    private void Update()
    {
        if (!firstDoorCompleted)
        {
            var isAllFirstDoorItemsGrabbed = true;
            foreach (var grabItem in firstDoorItems)
            {
                if (!grabItem.IsSnapped())
                {
                    isAllFirstDoorItemsGrabbed = false;
                    break;
                }
            }

            if (isAllFirstDoorItemsGrabbed)
            {
                firstDoorCompleted = true;
                OnFirstDoorComplete();
            }
        }

        if (!secondDoorCompleted)
        {
            var isAllSecondDoorItemsGrabbed = true;
            foreach (var grabItem in secondDoorItems)
            {
                if (!grabItem.IsSnapped())
                {
                    isAllSecondDoorItemsGrabbed = false;
                    break;
                }
            }

            if (isAllSecondDoorItemsGrabbed)
            {
                secondDoorCompleted = true;
                OnSecondDoorComplete();
            }
        }

        if (!thirdDoorCompleted)
        {
            var isAllThirdDoorItemsGrabbed = true;
            foreach (var grabItem in thirdDoorItems)
            {
                if (!grabItem.IsSnapped())
                {
                    isAllThirdDoorItemsGrabbed = false;
                    break;
                }
            }

            if (isAllThirdDoorItemsGrabbed)
            {
                thirdDoorCompleted = true;
                OnThirdDoorComplete();
            }
        }
    }

    private void OnFirstDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence();
    }

    private void OnSecondDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence();
    }

    private void OnThirdDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence();
    }
}