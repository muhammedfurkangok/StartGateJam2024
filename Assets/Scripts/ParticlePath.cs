using DG.Tweening;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    [Header("WAYPOINTS")]
    [SerializeField]
    Transform[] wps;

    [Space]

    [Header("PARTICLE EFFECT RB")]
    [SerializeField] Rigidbody rb;
    [Header("IMMOBILE particle system")]
    [SerializeField]
    private ParticleSystem immobileparticleSystem;

    [Space]

    [Header("PARAMETERS")]
    [SerializeField]
    float duration = 5f;
    [SerializeField]
    PathType pathType = PathType.Linear;
    [SerializeField]
    PathMode pathMode = PathMode.Full3D;
    [SerializeField]
    Color gizmoColor = Color.red;
    [SerializeField]
    Ease ease = Ease.Linear;
    [SerializeField]
    float lookAt = 0.01f;
    [SerializeField]
    sbyte loopCount = -1;

    [Space]

    [Header("DEBUG")]
    [SerializeField]
    Vector3[] waypoints;


    private Tween pathTween;
    private ParticleSystem mobileParticleSystem;
 

    private void Start()
    {
        rb.transform.position = wps[0].position;
        mobileParticleSystem = rb.gameObject.GetComponent<ParticleSystem>();
        waypoints = new Vector3[wps.Length];
        for (int i = 0; i < wps.Length; i++)
        {
            waypoints[i] = wps[i].position;
        }
        //Invoke(nameof(StartPathing), 8f);
        //Invoke(nameof(Kill), 15f);
    }

    public void StartPathing()
    {
        mobileParticleSystem.Play();
        pathTween = rb.DOPath(waypoints, duration, pathType, pathMode, 10, gizmoColor).SetEase(ease).SetLookAt(lookAt).SetLoops(loopCount);
        immobileparticleSystem.Stop();
        return;
    }

    public void Kill()
    {
        mobileParticleSystem.Stop();
        pathTween.Kill();
    }
}
