using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class DoorCompletionCheckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GrabItemPosition[] firstDoorItemPositions;
    [SerializeField] private GrabItemPosition[] secondDoorItemPositions;
    [SerializeField] private GrabItemPosition[] thirdDoorItemPositions;
    [SerializeField] private Patroller firstDoorParticle;
    [SerializeField] private Patroller secondDoorParticle;
    [SerializeField] private Patroller thirdDoorParticle;

    [Header("Completion References")]
    [SerializeField] private Transform completionDoor;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private Animator blinkAnimator;

    [Header("Completion Parameters")]
    [SerializeField] private float completionDoorMoveDuration;
    [SerializeField] private Ease completionDoorMoveEase;
    [SerializeField] private float playerLookAtDuration;
    [SerializeField] private Ease playerLookAtEase;

    [Header("Info")]
    [SerializeField] private bool firstDoorCompleted;
    [SerializeField] private bool secondDoorCompleted;
    [SerializeField] private bool thirdDoorCompleted;
    [SerializeField] private bool isAllDoorsCompleted;

    private void Update()
    {
        if (!firstDoorCompleted)
        {
            var doorMainType = DoorMainType.None;
            var isAllFirstDoorItemsGrabbed = true;
            foreach (var grabItemPosition in firstDoorItemPositions)
            {
                if (!grabItemPosition.IsCompleted())
                {
                    isAllFirstDoorItemsGrabbed = false;
                    break;
                }

                if (grabItemPosition.GetCurrentGrabItem().GetDoorMainType() != DoorMainType.None)
                {
                    doorMainType = grabItemPosition.GetCurrentGrabItem().GetDoorMainType();
                }
            }

            if (isAllFirstDoorItemsGrabbed)
            {
                if (doorMainType == DoorMainType.None) throw new Exception("DoorMainType is None");
                firstDoorCompleted = true;
                OnFirstDoorComplete(doorMainType);
            }
        }

        if (!secondDoorCompleted)
        {
            var doorMainType = DoorMainType.None;
            var isAllSecondDoorItemsGrabbed = true;
            foreach (var grabItemPosition in secondDoorItemPositions)
            {
                if (!grabItemPosition.IsCompleted())
                {
                    isAllSecondDoorItemsGrabbed = false;
                    break;
                }

                if (grabItemPosition.GetCurrentGrabItem().GetDoorMainType() != DoorMainType.None)
                {
                    doorMainType = grabItemPosition.GetCurrentGrabItem().GetDoorMainType();
                }
            }

            if (isAllSecondDoorItemsGrabbed)
            {
                if (doorMainType == DoorMainType.None) throw new Exception("DoorMainType is None");
                secondDoorCompleted = true;
                OnSecondDoorComplete(doorMainType);
            }
        }

        if (!thirdDoorCompleted)
        {
            var doorMainType = DoorMainType.None;
            var isAllThirdDoorItemsGrabbed = true;
            foreach (var grabItemPosition in thirdDoorItemPositions)
            {
                if (!grabItemPosition.IsCompleted())
                {
                    isAllThirdDoorItemsGrabbed = false;
                    break;
                }

                if (grabItemPosition.GetCurrentGrabItem().GetDoorMainType() != DoorMainType.None)
                {
                    doorMainType = grabItemPosition.GetCurrentGrabItem().GetDoorMainType();
                }
            }

            if (isAllThirdDoorItemsGrabbed)
            {
                if (doorMainType == DoorMainType.None) throw new Exception("DoorMainType is None");
                thirdDoorCompleted = true;
                OnThirdDoorComplete(doorMainType);
            }
        }

        if (firstDoorCompleted && secondDoorCompleted && thirdDoorCompleted && !isAllDoorsCompleted)
        {
            isAllDoorsCompleted = true;
            OnAllDoorsComplete().Forget();
        }
    }

    private void OnFirstDoorComplete(DoorMainType doorMainType)
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(GetVoiceTypeFromDoorMainType(doorMainType));
        firstDoorParticle.StartPathing();
    }

    private void OnSecondDoorComplete(DoorMainType doorMainType)
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(GetVoiceTypeFromDoorMainType(doorMainType));
        secondDoorParticle.StartPathing();
    }

    private void OnThirdDoorComplete(DoorMainType doorMainType)
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(GetVoiceTypeFromDoorMainType(doorMainType));
        thirdDoorParticle.StartPathing();
    }

    private async UniTask OnAllDoorsComplete()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");

        completionDoor.DOMoveY(0f, completionDoorMoveDuration)
            .SetEase(completionDoorMoveEase);

        var lookAtPosition = completionDoor.position;
        lookAtPosition.y = playerCamera.transform.position.y;
        await playerCamera.transform.DOLookAt(lookAtPosition, playerLookAtDuration)
            .SetEase(playerLookAtEase);

        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
    }

    private VoiceType GetVoiceTypeFromDoorMainType(DoorMainType doorMainType)
    {
        if (doorMainType == DoorMainType.Religion) return VoiceType.DoorRoomCross;
        if (doorMainType == DoorMainType.Dog) return VoiceType.DoorRoomBone;
        if (doorMainType == DoorMainType.Music) return VoiceType.DoorRoomMusic;
        throw new Exception("DoorMainType is None");
    }
}