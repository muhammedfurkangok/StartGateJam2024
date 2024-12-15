using System;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private GameObject tabletVisual;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private Image[] sprites;

    [Header("Parameters")]
    [SerializeField] private int intelligenceLevel;

    [Header("Info")]
    [SerializeField] private bool isTabletActive;

    public bool IsTabletActive() => isTabletActive;

    public static TabletManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        var tabletPosition = tabletVisual.transform.localPosition;
        tabletPosition.y = gameConstants.tabletDownLocalY;
        tabletVisual.transform.localPosition = tabletPosition;

        UpdateIntelligence(0);
    }

    private void Update()
    {
        if (PlayerController.Instance.IsInspecting() || PlayerGrabManager.Instance.IsHoldingItem()) return;
        if (!InputManager.Instance.IsTabletKeyDown()) return;
        
        ToggleTablet();
    }

    private void ToggleTablet()
    {
        tabletVisual.transform.DOKill();

        if (isTabletActive)
        {
            tabletVisual.transform.DOLocalMoveY(gameConstants.tabletDownLocalY, gameConstants.tabletMoveDuration)
                .SetEase(gameConstants.tabletMoveDownEase)
                .OnComplete(() => tabletVisual.gameObject.SetActive(false));
        }

        else
        {
            tabletVisual.gameObject.SetActive(true);
            tabletVisual.transform.DOLocalMoveY(gameConstants.tabletUpLocalY, gameConstants.tabletMoveDuration)
                .SetEase(gameConstants.tabletMoveUpEase);
        }

        isTabletActive = !isTabletActive;
    }

    public void UpdateIntelligence(int changeAmount)
    {
        intelligenceLevel = Mathf.Clamp(intelligenceLevel + changeAmount, gameConstants.minIntelligence, gameConstants.maxIntelligence);
        intelligenceText.text = intelligenceLevel.ToString("F0") + "%";

        var intelligenceIndex = intelligenceLevel / 10;
        for (var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            sprite.enabled = i < intelligenceIndex;
        }
    }
}