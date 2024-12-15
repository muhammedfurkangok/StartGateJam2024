using System;
using DG.Tweening;
using UnityEngine;

public class ClosetTrigger : MonoBehaviour
{
    public GameObject closetLeftDoor;
    public GameObject closetRightDoor;
    
    public bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            ChildRoomCutScene.Instance.ClosetPuzzleSolved();
            ClosetDoorsTrigger();
        }
    }

    private void ClosetDoorsTrigger()
    {
        closetLeftDoor.transform.DOLocalRotate(new Vector3(0, 90, 0), 1f).SetEase(Ease.InOutSine);
        closetRightDoor.transform.DOLocalRotate(new Vector3(0, -90, 0), 1f).SetEase(Ease.InOutSine);
    }
    
    
    
    
}