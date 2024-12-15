using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class ChildRoomCutScene : MonoBehaviour
{
    #region singleton

    public static ChildRoomCutScene Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    public Animator blinkAnimator;
    public CinemachineCamera cinemachineVirtualCamera;

    public GameObject table;
    public GameObject teddyBear;
    public GameObject BabyDollAndCard;

    public bool isteddyPuzzleSolved;
    public bool isPrismPuzzleSolved;
    public GameObject endSequenceLookPosition;
    public GameObject endSequenceDoor;

    private void Start()
    {
        SceneStartSequence();
    }
    
    private async void EndSequence()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");
        cinemachineVirtualCamera.transform.DOLookAt(endSequenceLookPosition.transform.position, 1f);
        endSequenceDoor.transform.DOLocalMoveY(0, 1f);
        await UniTask.WaitForSeconds(4f);
        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
    }


    public async void SceneStartSequence()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");
        VoiceAndSubtitleManager.Instance.Play(VoiceType.ChildRoom1);
        await UniTask.WaitForSeconds(3f);
        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
        
    }

    public async void PrismPuzzleSolved()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");
        cinemachineVirtualCamera.transform.DOLookAt(table.transform.position, 1f);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.ChildRoom2);
        await UniTask.WaitForSeconds(2f);
        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
        isPrismPuzzleSolved = true;
        if (isteddyPuzzleSolved && isPrismPuzzleSolved)
        {
            await UniTask.WaitForSeconds(1f);
            EndSequence();
        }
    }

    public async void TeddyPuzzleSolved()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");
        cinemachineVirtualCamera.transform.DOLookAt(teddyBear.transform.position, 1f);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.ChildRoom3);
        await UniTask.WaitForSeconds(1f);
        SoundManager.Instance.PlayOneShotSound(SoundType.BabyLaugh);
        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
        isteddyPuzzleSolved = true;
        if (isteddyPuzzleSolved && isPrismPuzzleSolved)
        {
            await UniTask.WaitForSeconds(1f);
            EndSequence();
        }
    }

    public async void ClosetPuzzleSolved()
    {
        InputManager.Instance.isInputOverride = true;
        blinkAnimator.SetTrigger("CutsceneOpen");
        cinemachineVirtualCamera.transform.DOLookAt(BabyDollAndCard.transform.position, 1f);
        VoiceAndSubtitleManager.Instance.Play(VoiceType.ChildRoom4);
        await UniTask.WaitForSeconds(3f);
        blinkAnimator.SetTrigger("CutsceneClose");
        InputManager.Instance.isInputOverride = false;
    }
}