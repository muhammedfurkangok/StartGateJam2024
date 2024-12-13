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

        [Header("Camera Shake Settings")]
        public float cameraShakeIntensity = 0.1f;
        public float cameraShakeSpeed = 2.0f;
    }
}