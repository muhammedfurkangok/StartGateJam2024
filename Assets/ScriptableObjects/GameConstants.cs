using UnityEngine;

namespace ScriptableObjects
{
    //[CreateAssetMenu(fileName = "GameConstants", menuName = "GameConstants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        [Header("Player Movement")]
        public float playerWalkSpeed;
        public float playerRunSpeed;
        public float playerAcceleration;
        public float playerDeceleration;
        public float playerLookSensitivity;
    }
}