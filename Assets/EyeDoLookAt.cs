using System.Collections;
using UnityEngine;

public class EyeDoLookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform eyePivot;
    [SerializeField] private Transform character; // Reference to the character
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private float minBlinkTime = 0.2f;
    [SerializeField] private float maxBlinkTime = 1f;

    private void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    private void Update()
    {
        // Calculate the direction from the eye pivot to the target
        Vector3 direction = (target.position - eyePivot.position).normalized;
        direction.x = 0; // Keep the eye pivot upright

        // Make the eye pivot look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        eyePivot.rotation = lookRotation;

        // Apply the character's rotation to the eye pivot
        transform.rotation = character.rotation * eyePivot.rotation;
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            float blinkTime = UnityEngine.Random.Range(minBlinkTime, maxBlinkTime);
            yield return new WaitForSeconds(blinkTime);
            characterAnimator.SetTrigger("Blink");
        }
    }
}