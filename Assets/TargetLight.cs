using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TargetLight : MonoBehaviour
{
    [SerializeField] private bool isCompleted;
    public bool isAnimDone;

    public GameObject lightObject;

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;
        if (other.CompareTag("Player"))
        {
            isCompleted = true;
            Debug.Log("Target reached!");
        }
    }

    public void SetIsCompleted(bool b)
    {
        isCompleted = b;
    }

    private void Update()
    {
        if (isCompleted && !isAnimDone)
        {
            isAnimDone = true;
            LightAnimTrigger();
        }
    }

    private void LightAnimTrigger()
    {
        lightObject.SetActive(true);
        ChildRoomCutScene.Instance.PrismPuzzleSolved();
    }
}