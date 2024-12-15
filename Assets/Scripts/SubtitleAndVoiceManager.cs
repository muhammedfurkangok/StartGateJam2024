using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public enum SubtitleType
{
    Hospital1,
    Hospital2,
    Eye1,
    Eye2,
    EyeCloseDistance,
    EyeDoNothing,
}

[Serializable]
public class SubtitleEntry
{
    public SubtitleType subtitleType;
    public string text;
    public AudioClip audioClip;
    public float displayDuration;
}

public class SubtitleAndVoiceManager : MonoBehaviour
{
    [Header("Subtitles")]
    [SerializeField] private List<SubtitleEntry> subtitles;

    [Header("References")]
    [SerializeField] private TypewriterByCharacter typewriter;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private AudioSource audioSource;

    private Coroutine subtitleCoroutine;

    public static SubtitleAndVoiceManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (subtitleText == null || audioSource == null)
        {
            Debug.LogError("Subtitle Text or AudioSource is not assigned.");
            return;
        }

        subtitleText.text = "";
    }

    public void PlaySubtitle(SubtitleType subtitleType)
    {
        var index = subtitles.FindIndex(x => x.subtitleType == subtitleType);
        if (index == -1)
        {
            Debug.LogError("Subtitle type not found.");
            return;
        }

        subtitleCoroutine = StartCoroutine(DisplaySubtitle(subtitles[index]));
    }

    private IEnumerator DisplaySubtitle(SubtitleEntry entry)
    {
        if (entry.audioClip != null)
        {
            audioSource.clip = entry.audioClip;
            audioSource.Play();
        }

        typewriter.ShowText(entry.text);

        var duration = Mathf.Max(entry.displayDuration, entry.audioClip != null ? entry.audioClip.length : 0);
        yield return new WaitForSeconds(duration);

        subtitleText.text = "";
    }

    public void StopSubtitle()
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
            subtitleCoroutine = null;
        }

        audioSource.Stop();
        subtitleText.text = "";
    }
}