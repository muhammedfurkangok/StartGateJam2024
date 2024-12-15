using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cutscenes
{
    public class EyeCutsceneManager : MonoBehaviour
    {
        private static readonly int Blink = Animator.StringToHash("Blink");

        [Header("References")]
        [SerializeField] private Animator playerEyeAnimator;

        [Header("Parameters")]
        [SerializeField] private float blinkDuration;

        private void Start()
        {
            PlayCutscene().Forget();
        }

        private async UniTask PlayCutscene()
        {
            playerEyeAnimator.SetTrigger(Blink);
            InputManager.Instance.isInputOverride = true;

            await UniTask.WaitForSeconds(blinkDuration);

            InputManager.Instance.isInputOverride = false;
        }
    }
}