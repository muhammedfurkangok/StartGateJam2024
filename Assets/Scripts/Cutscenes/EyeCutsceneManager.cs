﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Cutscenes
{
    public class EyeCutsceneManager : MonoBehaviour
    {
        private static readonly int Blink = Animator.StringToHash("Blink");

        [Header("References")] [SerializeField]
        private Animator playerEyeAnimator;

        [SerializeField] private CinemachineCamera playerCamera;
        [SerializeField] private GameObject player;

        [Header("Look Parameters")] [SerializeField]
        private float blinkDuration;

        [SerializeField] private Vector3[] rightLeftLookRotations;
        [SerializeField] private float[] rightLeftLookDurations;
        [SerializeField] private Ease[] rightLeftLookEases;
        [SerializeField] private float lookLastWaitDuration;

        [Header("Tablet Parameters")] [SerializeField]
        private float tabletUpPosition;

        [SerializeField] private float tabletUpDuration;
        [SerializeField] private Ease tabletUpEase;
        [SerializeField] private float tabletWaitDuration;
        [SerializeField] private float tabletDownDuration;
        [SerializeField] private Ease tabletDownEase;
        [SerializeField] private float tabletLastWaitDuration;
        [SerializeField] private GameObject doorLookPos;

        [Header("Shake and Door Parameters")] [SerializeField]
        private Transform[] doors;

        [SerializeField] private float doorsUpPosition;
        [SerializeField] private float doorsUpDuration;
        [SerializeField] private Ease doorsUpEase;

        private void Start()
        {
            PlayCutscene().Forget();
        }

        private async UniTask PlayCutscene()
        {
            playerEyeAnimator.SetTrigger(Blink);
            InputManager.Instance.isInputOverride = true;
            VoiceAndSubtitleManager.Instance.Play(VoiceType.Eye1);
            await UniTask.WaitForSeconds(blinkDuration);

            for (var i = 0; i < rightLeftLookRotations.Length; i++)
            {
                await playerCamera.transform.DOLocalRotate(rightLeftLookRotations[i], rightLeftLookDurations[i])
                    .SetEase(rightLeftLookEases[i]);
            }

            await UniTask.WaitForSeconds(lookLastWaitDuration);

            var tabletStartPositionY = TabletManager.Instance.GetTabletVisual().localPosition.y;

            VoiceAndSubtitleManager.Instance.Play(VoiceType.Eye2);

            await TabletManager.Instance.GetTabletVisual().DOLocalMoveY(tabletUpPosition, tabletUpDuration)
                .SetEase(tabletUpEase);

            await UniTask.WaitForSeconds(tabletWaitDuration);

            await TabletManager.Instance.GetTabletVisual().DOLocalMoveY(tabletStartPositionY, tabletDownDuration)
                .SetEase(tabletDownEase);

            await UniTask.WaitForSeconds(tabletLastWaitDuration);
         
            await playerCamera.transform.DOLookAt(doorLookPos.transform.position, 0.5f);
            playerEyeAnimator.SetTrigger("CutsceneOpen");
            SoundManager.Instance.PlayOneShotSound(SoundType.Explosion);
            DOVirtual.Float(0, 1, 0.75f, value => playerCamera.Lens.FieldOfView = Mathf.Lerp(60, 45, value));
            playerCamera.transform.DOShakeRotation( 2f, 1f, 10, 35f, false);
            foreach (var door in doors)
            {
                door.DOLocalMoveY(doorsUpPosition, doorsUpDuration)
                    .SetEase(doorsUpEase);
            }

            await UniTask.WaitForSeconds(doorsUpDuration);
            DOVirtual.Float(0, 1, 0.75f, value => playerCamera.Lens.FieldOfView = Mathf.Lerp(45, 60, value));
            playerEyeAnimator.SetTrigger("CutsceneClose");

            InputManager.Instance.isInputOverride = false;
        }
    }
}