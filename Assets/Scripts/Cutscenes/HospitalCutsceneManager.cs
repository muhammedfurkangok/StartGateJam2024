using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Cutscenes
{
    public class HospitalCutsceneManager : MonoBehaviour
    {
        private static readonly int CutsceneOpen = Animator.StringToHash("CutsceneOpen");
        private static readonly int CutsceneClose = Animator.StringToHash("CutsceneClose");

        [Header("Hospital Scene References")]
        [SerializeField] private Animator playerEyeAnimator;
        [SerializeField] private CinemachineCamera playerCamera;

        [Header("Hospital Scene Parameters")]
        [SerializeField] private float lookUpInputTarget;
        [SerializeField] private float lookUpDuration;
        [SerializeField] private float lookDownInputTarget;
        [SerializeField] private float lookDownDuration;
        [SerializeField] private Ease lookDownEase;
        [SerializeField] private float lookToZeroDuration;
        [SerializeField] private Ease lookToZeroEase;
        [SerializeField] private float waitBetweenLookAndWalkDuration;
        [SerializeField] private float walkDuration;
        [SerializeField] private float tabletWaitDuration;
        [SerializeField] private float tabletWaitDuration2;
        [SerializeField] private float lookRightInputTarget;
        [SerializeField] private float lookRightDuration;
        [SerializeField] private Ease lookRightEase;
        [SerializeField] private float lookToZeroDuration2;
        [SerializeField] private Ease lookToZeroEase2;
        [SerializeField] private float lookRightWaitDuration;
        [SerializeField] private float lookLeftInputTarget;
        [SerializeField] private float lookLeftDuration;
        [SerializeField] private Ease lookLeftEase;
        [SerializeField] private float lookToZeroDuration3;
        [SerializeField] private Ease lookToZeroEase3;
        [SerializeField] private float voiceWaitDuration;
        [SerializeField] private float wideFov;
        [SerializeField] private float wideFovDuration;
        [SerializeField] private Ease wideFovEase;
        [SerializeField] private float narrowFov;
        [SerializeField] private float narrowFovDuration;
        [SerializeField] private Ease narrowFovEase;

        private void Start()
        {
            PlayHospitalSceneCutscene().Forget();
        }

        private async UniTask PlayHospitalSceneCutscene()
        {
            playerEyeAnimator.SetTrigger(CutsceneOpen);
            InputManager.Instance.isInputOverride = true;

            InputManager.Instance.overrideLookInput = Vector2.up * lookUpInputTarget;
            await UniTask.WaitForSeconds(lookUpDuration);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.y, lookDownInputTarget, lookDownDuration,value =>
                    InputManager.Instance.overrideLookInput = new Vector2(InputManager.Instance.overrideLookInput.x, value))
                .SetEase(lookDownEase);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.y, 0, lookToZeroDuration, value =>
                    InputManager.Instance.overrideLookInput = new Vector2(InputManager.Instance.overrideLookInput.x, value))
                .SetEase(lookToZeroEase);

            await UniTask.WaitForSeconds(waitBetweenLookAndWalkDuration);

            InputManager.Instance.overrideMoveInput = Vector2.up;

            await UniTask.WaitForSeconds(walkDuration);

            InputManager.Instance.overrideMoveInput = Vector2.zero;
            InputManager.Instance.OverrideTabletKeyDown().Forget();

            await UniTask.WaitForSeconds(tabletWaitDuration);

            InputManager.Instance.OverrideTabletKeyDown().Forget();

            await UniTask.WaitForSeconds(tabletWaitDuration2);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.x, lookRightInputTarget, lookRightDuration, value =>
                    InputManager.Instance.overrideLookInput = new Vector2(value, InputManager.Instance.overrideLookInput.y))
                .SetEase(lookRightEase);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.x, 0, lookToZeroDuration2, value =>
                    InputManager.Instance.overrideLookInput = new Vector2(value, InputManager.Instance.overrideLookInput.y))
                .SetEase(lookToZeroEase2);

            await UniTask.WaitForSeconds(lookRightWaitDuration);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.x, lookLeftInputTarget, lookLeftDuration, value =>
                    InputManager.Instance.overrideLookInput = new Vector2(value, InputManager.Instance.overrideLookInput.y))
                .SetEase(lookLeftEase);

            await DOVirtual.Float(InputManager.Instance.overrideLookInput.x, 0, lookToZeroDuration3, value =>
                    InputManager.Instance.overrideLookInput = new Vector2(value, InputManager.Instance.overrideLookInput.y))
                .SetEase(lookToZeroEase3);

            SubtitleAndVoiceManager.Instance.PlaySubtitle(SubtitleType.Hospital1);

            await UniTask.WaitForSeconds(voiceWaitDuration);

            await DOVirtual.Float(playerCamera.Lens.FieldOfView, wideFov, wideFovDuration, value =>
                    playerCamera.Lens.FieldOfView = value)
                .SetEase(wideFovEase);

            await DOVirtual.Float(playerCamera.Lens.FieldOfView, narrowFov, narrowFovDuration, value =>
                    playerCamera.Lens.FieldOfView = value)
                .SetEase(narrowFovEase);
        }
    }
}