using UnityEngine;

namespace Cutscenes
{
    public class LessEyeCutsceneManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Door firstDoor;
        [SerializeField] private int firstDoorIndex;
        [SerializeField] private Door secondDoor;
        [SerializeField] private int secondDoorIndex;

        private void Start()
        {
            var firstScene = PlayerPrefs.GetInt("first_scene", 0);

            if (firstScene == firstDoorIndex)
            {
                firstDoor.gameObject.SetActive(false);
            }

            else if (firstScene == secondDoorIndex)
            {
                secondDoor.gameObject.SetActive(false);
            }
        }
    }
}