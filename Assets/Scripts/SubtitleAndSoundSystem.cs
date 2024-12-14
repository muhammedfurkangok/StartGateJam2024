using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SubtitleAndSoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class SubtitleEntry
    {
        public string text;
        public AudioClip audioClip;
        public float displayDuration;
        public UnityEvent onDisplay;
    }

    public TypewriterByCharacter typewriter;
    public TextMeshProUGUI subtitleText;
    public AudioSource audioSource;
    public List<SubtitleEntry> subtitles;

    private Coroutine subtitleCoroutine;

    void Start()
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

        float duration = Mathf.Max(entry.displayDuration, entry.audioClip != null ? entry.audioClip.length : 0);
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