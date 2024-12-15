using System;
using Unity.VisualScripting;
using UnityEngine;

public class Teddy : MonoBehaviour
{
    [SerializeField] private GrabItemPosition grabItem;
    public bool isPuzzleDone;
    public AudioSource audioSource;
    private void Update()
    {
        if (grabItem.IsCompleted() && !isPuzzleDone)
        {
            isPuzzleDone = true;
            TeddyPuzzleDone();
        }
    }

    private void TeddyPuzzleDone()
    {
        audioSource.clip = null;
        ChildRoomCutScene.Instance.TeddyPuzzleSolved();
    }
}
