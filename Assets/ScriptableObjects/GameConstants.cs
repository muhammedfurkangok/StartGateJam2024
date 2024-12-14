using DG.Tweening;
using UnityEngine;

namespace ScriptableObjects
{
    //[CreateAssetMenu(fileName = "GameConstants", menuName = "GameConstants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        [Header("Player Movement")]
        public float playerWalkSpeed;
        public float playerRunSpeed;
        public float playerWalkAcceleration;
        public float playerRunAcceleration;
        public float playerDeceleration;
        public float playerLookSensitivity;
        public float playerInspectSensitivity;

        [Header("Player Grab")]
        public LayerMask grabLayer;
        public float grabRange;
        public float grabForce;
        public float grabAngularForce;
        public float grabReleaseDeceleration;
        public float grabItemEnterInspectDuration;
        public Ease grabItemEnterInspectEase;
        public float grabItemExitInspectDuration;
        public Ease grabItemExitInspectEase;
        public Ease grabItemExitInspectRotationEase;
    }
}