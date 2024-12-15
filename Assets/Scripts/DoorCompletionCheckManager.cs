using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
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

        if (firstDoorCompleted && secondDoorCompleted && thirdDoorCompleted && !isAllDoorsCompleted)
        {
            isAllDoorsCompleted = true;
            OnAllDoorsComplete().Forget();
        }

        if (Input.GetKeyDown(KeyCode.P)) OnAllDoorsComplete().Forget();
    }

    private void OnFirstDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.DoorRoomCross);
    }

    private void OnSecondDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.DoorRoomBone);
    }

    private void OnThirdDoorComplete()
    {
        TabletManager.Instance.IncreaseIntelligence(1);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.DoorRoomMusic);
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
}