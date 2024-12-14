using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusManager : MonoBehaviour
{
    [Header("Zekâ Seviyesi Ayarları")]
    [Range(0, 100)] public int intelligenceLevel = 100;
    public int maxIntelligence = 100;
    public int minIntelligence = 0;

    [Header("Tablet ve UI Elemanları")]
    public GameObject tablet;
    public TextMeshProUGUI intelligenceText;
    public Image[] sprites;
    public float fadeDuration = 0.5f;

    private bool tabletActive = false;
    private Vector3 initialTabletPosition;

    private void Start()
    {
        initialTabletPosition = tablet.transform.localPosition;
        tablet.transform.localPosition = new Vector3(initialTabletPosition.x, -Screen.height, initialTabletPosition.z);
        tablet.SetActive(false);

        foreach (var sprite in sprites)
        {
            sprite.canvasRenderer.SetAlpha(0f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleTablet();
        }

        if (tabletActive)
        {
            UpdateTablet();
        }
    }

    public void IncreaseIntelligence(int amount)
    {
        intelligenceLevel = Mathf.Clamp(intelligenceLevel + amount, minIntelligence, maxIntelligence);
        UpdateSprites();
        UpdateTablet();
    }

    public void DecreaseIntelligence(int amount)
    {
        intelligenceLevel = Mathf.Clamp(intelligenceLevel - amount, minIntelligence, maxIntelligence);
        UpdateSprites();
        UpdateTablet();
    }

    private void ToggleTablet()
    {
        tablet.transform.DOKill(); // Kill any ongoing animations

        if (tabletActive)
        {
            tablet.transform.DOLocalMoveY(-Screen.height, 0.5f).OnComplete(() => tablet.SetActive(false));
        }
        else
        {
            tablet.SetActive(true);
            tablet.transform.DOLocalMoveY(initialTabletPosition.y, 0.5f).SetEase(Ease.OutBounce); // Add a simple bounce animation
        }
        tabletActive = !tabletActive;
    }

    private void UpdateTablet()
    {
        float percentage = (float)intelligenceLevel / maxIntelligence * 100;
        intelligenceText.text = "Zekâ Seviyesi: " + percentage.ToString("F0") + "%";
        UpdateSprites();
    }

    private void UpdateSprites()
    {
        float percentage = (float)intelligenceLevel / maxIntelligence;

        for (int i = 0; i < sprites.Length; i++)
        {
            float targetAlpha = percentage > (i + 1) / (float)sprites.Length ? 1f : 0f;
            sprites[i].CrossFadeAlpha(targetAlpha, fadeDuration, false);
        }
    }
}