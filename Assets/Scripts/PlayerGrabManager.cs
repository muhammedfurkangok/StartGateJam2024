using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGrabManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Camera mainCamera;

    [Header("Info")]
    [SerializeField] private GrabItem currentGrabItem;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.IsInteractKeyDown())
        {

        }

        var crosshairRay = mainCamera.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out var crosshairHit, gameConstants.grabRange, gameConstants.grabLayer))
        {
            currentGrabItem = null;
            UpdateCrosshairAlpha();

            return;
        }

        currentGrabItem = crosshairHit.collider.GetComponent<GrabItem>();
        UpdateCrosshairAlpha();

        if (currentGrabItem == null) return;


    }

    private void UpdateCrosshairAlpha()
    {
        var crosshairImageColor = crosshairImage.color;
        crosshairImageColor.a = currentGrabItem != null ? 1f : 0.25f;
        crosshairImage.color = crosshairImageColor;
    }
}