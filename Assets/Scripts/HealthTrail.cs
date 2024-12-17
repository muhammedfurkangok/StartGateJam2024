using UnityEngine;
using DG.Tweening;

public class HealthStatusPath : MonoBehaviour
{
    public Transform healthTrail; // Sağlık durumunu gösterecek obje (Trail Renderer veya başka bir obje)
    public Transform[] waypoints; // Geçilecek noktalar
    public float duration = 3f; // Hareket süresi
    public PathType pathType = PathType.CatmullRom; // Bezier veya başka tipte path
    public Ease easeType = Ease.InOutSine; // Smooth hareket için easing
    public float restartDelay = 0.5f; // Yeniden başlatma gecikmesi
    public bool useFadeEffect = true; // Fade-in/out efekti eklemek için

    private Renderer trailRenderer;

    private void Start()
    {
        if (healthTrail.TryGetComponent(out Renderer renderer))
        {
            trailRenderer = renderer;
        }

        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        // Waypoint pozisyonlarını Vector3 array'ine dönüştür
        Vector3[] points = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            points[i] = waypoints[i].position;
        }

        // DOTween ile belirtilen path boyunca hareket ettir
        healthTrail.DOPath(points, duration, pathType)
            .SetEase(easeType) // Daha yumuşak hareket
            .OnComplete(() => 
            {
                // Trail son noktaya ulaştığında işlemler
                if (useFadeEffect)
                {
                    FadeOutTrail(() => ResetTrail());
                }
                else
                {
                    ResetTrail();
                }
            });
    }

    void ResetTrail()
    {
        // Trail'i başlangıç pozisyonuna döndür ve gizle
        healthTrail.gameObject.SetActive(false);
        healthTrail.position = waypoints[0].position;

        // Yeniden başlatmayı gecikme ile çağır
        DOVirtual.DelayedCall(restartDelay, () =>
        {
            if (useFadeEffect)
            {
                FadeInTrail(() => MoveAlongPath());
            }
            else
            {
                healthTrail.gameObject.SetActive(true);
                MoveAlongPath();
            }
        });
    }

    void FadeOutTrail(TweenCallback onComplete)
    {
        if (trailRenderer != null)
        {
            trailRenderer.material.DOFade(0, 0.3f).OnComplete(onComplete);
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    void FadeInTrail(TweenCallback onComplete)
    {
        healthTrail.gameObject.SetActive(true);
        if (trailRenderer != null)
        {
            trailRenderer.material.DOFade(1, 0.3f).OnComplete(onComplete);
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}
