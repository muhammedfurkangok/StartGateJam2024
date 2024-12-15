using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes
{
    public class EyeFinalCutsceneManager : MonoBehaviour
    {
        private static readonly int CutsceneOpen = Animator.StringToHash("CutsceneOpen");
        private static readonly int CutsceneClose = Animator.StringToHash("CutsceneClose");

        [Header("References")]
        [SerializeField] private Transform finalEye;
        [SerializeField] private Animator playerEyeAnimator;
        [SerializeField] private CinemachineCamera playerCamera;
        [SerializeField] private Image fadeToBlackImage;
        [SerializeField] private TextMeshProUGUI finalText;
        [SerializeField] private TextMeshProUGUI thankYouText;
        [SerializeField] private Image crosshair;

        [Header("Look Up-Down Parameters")]
        [SerializeField] private Vector3 lookUpTarget;
        [SerializeField] private float lookUpDuration;
        [SerializeField] private Ease lookUpEase;
        [SerializeField] private Vector3 lookDownTarget;
        [SerializeField] private float lookDownDuration;
        [SerializeField] private Ease lookDownEase;
        [SerializeField] private float waitBetweenLookAndFinalDuration;
        [SerializeField] private float finalEyeDuration;
        [SerializeField] private Ease finalEyeEase;
        [SerializeField] private float waitBetweenEyeAndFadeDuration;
        [SerializeField] private float fadeToBlackDuration;
        [SerializeField] private Ease fadeToBlackEase;
        [SerializeField] private float finalTextFadeInDuration;
        [SerializeField] private Ease finalTextFadeInEase;
        [SerializeField] private float thankYouTextFadeInDuration;
        [SerializeField] private Ease thankYouTextFadeInEase;

        private void Start()
        {
            Cutscene().Forget();
        }

        private async UniTask Cutscene()
        {
            playerEyeAnimator.SetTrigger(CutsceneOpen);
            InputManager.Instance.isInputOverride = true;
            VoiceAndSubtitleManager.Instance.Play(VoiceType.LastRoom);

            await playerCamera.transform.DORotate(lookUpTarget, lookUpDuration)
                .SetEase(lookUpEase);

            await playerCamera.transform.DORotate(lookDownTarget, lookDownDuration)
                .SetEase(lookDownEase);

            await UniTask.WaitForSeconds(waitBetweenLookAndFinalDuration);

            await finalEye.DOScale(0f, finalEyeDuration)
                .SetEase(finalEyeEase);

            await UniTask.WaitForSeconds(waitBetweenEyeAndFadeDuration);

            crosshair.enabled = false;

            await fadeToBlackImage.DOFade(1f, fadeToBlackDuration)
                .SetEase(fadeToBlackEase);

            await finalText.DOFade(1f, finalTextFadeInDuration)
                .SetEase(finalTextFadeInEase);

            await thankYouText.DOFade(1f, thankYouTextFadeInDuration)
                .SetEase(thankYouTextFadeInEase);
        }
    }
}