using System;
using UnityEngine;

public class TargetLight : MonoBehaviour
{
   [SerializeField] private bool isCompleted;

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
}
