using DG.Tweening;
using UnityEngine;

namespace ScriptableObjects
{
    public enum GrabItemType
    {
        Cube,
        LightPrism,
        Screwdriver,
        Screw,
        DoorMain,
        DoorFrameTop,
        DoorFrame0,
        DoorFrame1,
        DoorHandleInside,
        DoorHandleOutside,
        DoorHingeTop,
        DoorHingeDown,
    }

    //[CreateAssetMenu(fileName = "GameConstants", menuName = "GameConstants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        [Header("Player Movement")]
        public float playerWalkSpeed;
        public float playerRunSpeed;
        public float playerWalkAcceleration;
        public float playerRunAcceleration;
        public float playerDeceleration;

        [Header("Player Look")]
        public float playerMinLook;
        public float playerMaxLook;
        public float playerLookSensitivity;
        public float playerInspectSensitivity;

        [Header("Player Noise")]
        public float playerIdleNoiseAmplitude;
        public float playerWalkNoiseAmplitude;
        public float playerRunNoiseAmplitude;
        public float playerIdleNoiseFrequency;
        public float playerWalkNoiseFrequency;
        public float playerRunNoiseFrequency;
        public float playerWalkNoiseChangeSpeed;
        public float playerRunNoiseChangeSpeed;

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
        public float grabThrowForce;
        public LayerMask grabItemExcludeLayers;

        [Header("Tablet")]
        public int minIntelligence;
        public int maxIntelligence;
        public float tabletMoveDuration;
        public Ease tabletMoveUpEase;
        public Ease tabletMoveDownEase;
        public float tabletUpLocalY;
        public float tabletDownLocalY;

        [Header("Grab Item Snap")]
        public float grabItemSnapMaxVelocity;
        public float grabItemSnapDuration;
        public Ease grabItemSnapEase;
        public Ease grabItemSnapRotationEase;
    }
}