using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cutscenes
{
    public class DoorCutscene : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator blinkAnimator;

        [Header("Parameters")]
        [SerializeField] private float waitTime;

        private void Start()
        {
            SceneStartSequence().Forget();
        }

        private async UniTask SceneStartSequence()
        {
            InputManager.Instance.isInputOverride = true;
            blinkAnimator.SetTrigger("CutsceneOpen");

            VoiceAndSubtitleManager.Instance.Play(VoiceType.DoorRoomEnter);
            await UniTask.WaitForSeconds(waitTime);

            blinkAnimator.SetTrigger("CutsceneClose");
            InputManager.Instance.isInputOverride = false;

        }
    }
}