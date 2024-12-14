using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubtitleAndVoiceManager : MonoBehaviour
{
    [Serializable]
    public class SubtitleEntry
    {
        public string text;
        public AudioClip audioClip;
        public float displayDuration;
        public UnityEvent onDisplay;
    }

    [Header("Subtitles")]
    [SerializeField] private List<SubtitleEntry> subtitles;

    [Header("References")]
    [SerializeField] private TypewriterByCharacter typewriter;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private AudioSource audioSource;

    private Coroutine subtitleCoroutine;

    private void Start()
    {
        if (subtitleText == null || audioSource == null)
        {
            Debug.LogError("Subtitle Text or AudioSource is not assigned.");
            return;
        }

        subtitleText.text = "";

        PlaySubtitle(0);
    }
    
    public void PlaySubtitle(int index)
    {
        if (index < 0 || index >= subtitles.Count)
        {
            Debug.LogError("Invalid subtitle index.");
            return;
        }

        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
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
        entry.onDisplay.Invoke();
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