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

        foreach (var sprite in sprites)
        {
            sprite.canvasRenderer.SetAlpha(0f);
        }
    }

    private void Update()
    {
        if (InputManager.Instance.IsTabletKeyDown())
        {
            ToggleTablet();
        }
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

    public void IncreaseIntelligence(int amount)
    {
        intelligenceLevel = Mathf.Clamp(intelligenceLevel + amount, gameConstants.minIntelligence, gameConstants.maxIntelligence);
        intelligenceText.text = intelligenceLevel.ToString("F0") + "%";
    }

    public void DecreaseIntelligence(int amount)
    {
        intelligenceLevel = Mathf.Clamp(intelligenceLevel - amount, gameConstants.minIntelligence, gameConstants.maxIntelligence);
        intelligenceText.text = intelligenceLevel.ToString("F0") + "%";
    }
}