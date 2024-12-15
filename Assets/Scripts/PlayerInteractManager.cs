using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Camera mainCamera;

    public static PlayerInteractManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        try
        {
            print("-1");
            if (!InputManager.Instance.IsInteractKeyDown()) return;
            print("0");

            var crosshairRay = mainCamera.ScreenPointToRay(crosshairImage.rectTransform.position);
            if (!Physics.Raycast(crosshairRay, out var crosshairHit, gameConstants.playerInteractRange, gameConstants.playerInteractLayer)) return;

            print("1");

            var door = crosshairHit.collider.transform.parent.parent.GetComponent<Door>();
            if (door == null)
            {
                print("2");
                door = crosshairHit.collider.transform.parent.parent.parent.GetComponent<Door>();
                if (door == null) return;
            }

            print("3");
            door.ToggleDoor();
        }

        catch { }
    }
}