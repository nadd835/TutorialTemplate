using System.Collections;
using UnityEngine;
using Crosstales.RTVoice;
using TMPro;

public class SequenceStep : MonoBehaviour
{
    [Header("Step Control")]
    public SequenceController sequenceManager;

    [Header("Voice Over")]
    public TMP_Text[] voiceText;
    public AudioSource audioSource;

    public void NextStep()
    {
        if (sequenceManager != null)
        {
            sequenceManager.NextStep();
        }
    }
    public void PlayVoiceOver()
    {
        StopAllCoroutines(); 
        StartCoroutine(PlayVoiceOverSequentially());
    }

    private IEnumerator PlayVoiceOverSequentially()
    {
        foreach (TMP_Text text in voiceText)
        {
            if (text != null && !string.IsNullOrWhiteSpace(text.text))
            {
                Speaker.Speak(text.text, audioSource);
                float estimatedDuration = EstimateSpeechDuration(text.text);
                yield return new WaitForSeconds(estimatedDuration);
            }
        }
    }

    private float EstimateSpeechDuration(string text)
    {
        return Mathf.Clamp(text.Length / 13f, 1f, 10f);
    }
}
