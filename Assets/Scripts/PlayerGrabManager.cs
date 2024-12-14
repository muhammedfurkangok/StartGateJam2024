using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGrabManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform holdPosition;

    [Header("Info")]
    [SerializeField] private GrabItem lookingGrabItem;
    [SerializeField] private GrabItem currentGrabItem;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var crosshairRay = mainCamera.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out var crosshairHit, gameConstants.grabRange, gameConstants.grabLayer))
        {
            lookingGrabItem = null;
            UpdateCrosshairAlpha();

            return;
        }

        lookingGrabItem = crosshairHit.collider.GetComponent<GrabItem>();
        UpdateCrosshairAlpha();

        if (!PlayerInputManager.Instance.IsInteractKeyDown()) return;

        if (currentGrabItem != null)
        {
            currentGrabItem.SetTarget(null);
            currentGrabItem = null;
        }

        else if (lookingGrabItem != null)
        {
            currentGrabItem = lookingGrabItem;
            currentGrabItem.SetTarget(holdPosition);
        }
    }

    private void UpdateCrosshairAlpha()
    {
        var crosshairImageColor = crosshairImage.color;
        crosshairImageColor.a = lookingGrabItem != null ? 1f : 0.25f;
        crosshairImage.color = crosshairImageColor;
    }
}