using System;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private bool isPlayingCompleteAnimation;

    public Transform GetTabletVisual() => tabletVisual.transform;
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

        intelligenceLevel = 30;
        intelligenceText.text = intelligenceLevel.ToString("F0") + "%";
    }

    private void Update()
    {
        if (PlayerController.Instance.IsInspecting() || PlayerGrabManager.Instance.IsHoldingItem()) return;
        if (!InputManager.Instance.IsTabletKeyDown()) return;

        ToggleTablet(false);
    }

    private void ToggleTablet(bool isComplete)
    {
        if (isPlayingCompleteAnimation && !isComplete) return;
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
                .SetEase(gameConstants.tabletMoveUpEase)
                .OnComplete(async () =>
                {
                    if (!isComplete) return;
                    await UniTask.WaitForSeconds(gameConstants.tabletAlphaDuration);
                    ToggleTablet(true);
                });
        }

        isTabletActive = !isTabletActive;
    }

    private void IncreaseIntelligence()
    {
        intelligenceLevel += 10;
        intelligenceText.text = intelligenceLevel.ToString("F0") + "%";

        var intelligenceIndex = intelligenceLevel / 10;
        for (var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            if (i > intelligenceIndex) sprite.DOFade(1, gameConstants.tabletAlphaDuration).SetEase(gameConstants.tabletAlphaEase);
        }

        ToggleTablet(true);

        SoundManager.Instance.PlayOneShotSound(SoundType.Intel);
    }
}