using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cutscenes
{
    public class HospitalCutsceneManager : MonoBehaviour
    {
        private static readonly int CutsceneOpen = Animator.StringToHash("CutsceneOpen");
        private static readonly int CutsceneClose = Animator.StringToHash("CutsceneClose");

        [Header("References")] [SerializeField]
        private Animator playerEyeAnimator;

        [SerializeField] private CinemachineCamera playerCamera;

        [Header("Look Up-Down and Walk Parameters")] [SerializeField]
        private Vector3 lookUpTarget;

        [SerializeField] private float lookUpDuration;
        [SerializeField] private Ease lookUpEase;
        [SerializeField] private Vector3 lookDownTarget;
        [SerializeField] private float lookDownDuration;
        [SerializeField] private Ease lookDownEase;
        [SerializeField] private float waitBetweenLookAndWalkDuration;
        [SerializeField] private float walkDuration;

        [Header("Tablet Parameters")] [SerializeField]
        private float tabletUpPosition;

        [SerializeField] private Transform tabletDownPosition;
        [SerializeField] private Transform tabletUpPositionx;
        [SerializeField] private float tabletUpDuration;
        [SerializeField] private Ease tabletUpEase;
        [SerializeField] private float tabletWaitDuration;
        [SerializeField] private float tabletDownDuration;
        [SerializeField] private Ease tabletDownEase;
        [SerializeField] private float waitBetweenTabletAndHealthCheckDuration;

        [Header("Look Down Parameters")] [SerializeField]
        private Transform lookRightTarget;

        [SerializeField] private float lookRightDuration;
        [SerializeField] private Ease lookRightEase;
        [SerializeField] private float waitBetweenLookRightAndLookLeftDuration;
        [SerializeField] private Transform lookLeftTarget;
        [SerializeField] private float lookLeftDuration;
        [SerializeField] private Ease lookLeftEase;
        [SerializeField] private float waitBetweenHealthCheckAndVoiceDuration;

        [Header("Voice and Fov Parameters")] [SerializeField]
        private float voiceWaitDuration;

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

            await playerCamera.transform.DORotate(lookUpTarget, lookUpDuration)
                .SetEase(lookUpEase);

            await playerCamera.transform.DORotate(lookDownTarget, lookDownDuration)
                .SetEase(lookDownEase);

            await UniTask.WaitForSeconds(waitBetweenLookAndWalkDuration);

            InputManager.Instance.overrideMoveInput = Vector2.up;

            VoiceAndSubtitleManager.Instance.Play(VoiceType.Hospital1);
            await UniTask.WaitForSeconds(walkDuration);

            InputManager.Instance.overrideMoveInput = Vector2.zero;

            await playerCamera.transform.DOLookAt(lookRightTarget.position, lookRightDuration)
                .SetEase(lookRightEase);

            await UniTask.WaitForSeconds(waitBetweenLookRightAndLookLeftDuration);

            await playerCamera.transform.DOLookAt(lookLeftTarget.position, lookLeftDuration)
                .SetEase(lookLeftEase);

            await UniTask.WaitForSeconds(waitBetweenHealthCheckAndVoiceDuration);
            lookLeftTarget.transform.DOJump(tabletDownPosition.position, 0.5f, 1, 0.65f).OnComplete(() =>
            {
                playerCamera.transform.DOLookAt(lookRightTarget.transform.position, 0.5f).SetEase(Ease.InOutSine);
            });


            await UniTask.WaitForSeconds(voiceWaitDuration);

            await DOVirtual.Float(playerCamera.Lens.FieldOfView, wideFov, wideFovDuration, value =>
                    playerCamera.Lens.FieldOfView = value)
                .SetEase(wideFovEase);

            SoundManager.Instance.PlayOneShotSound(SoundType.Fov);

            await DOVirtual.Float(playerCamera.Lens.FieldOfView, narrowFov, narrowFovDuration, value =>
                    playerCamera.Lens.FieldOfView = value)
                .SetEase(narrowFovEase);

            SceneManager.LoadScene(1);
        }
    }
}