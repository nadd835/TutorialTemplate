using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceStep : MonoBehaviour
{
    [Header("Step Control")]
    public TutorialSequenceController sequenceManager;

    [Header("Voice Over")]
    public AudioSource audioSource;
    public AudioClip[] voiceClips;

    private TutorialNavigationController navigationController;

    private void Awake()
    {
        navigationController = FindObjectOfType<TutorialNavigationController>();
    }

    public void NextStep()
    {
        if (navigationController != null)
        {
            navigationController.Next();
        }
        else if (sequenceManager != null)
        {
            sequenceManager.NextStep();
        }
    }

    public void PreviousStep()
    {
        // Stop voice over when going to previous step
        StopVoiceOver();
        
        if (navigationController != null)
        {
            navigationController.Previous();
        }
        else if (sequenceManager != null)
        {
            sequenceManager.PreviousStep();
        }
    }

    public void PlayVoiceOver()
    {
        StopAllCoroutines();
        StartCoroutine(PlayVoiceOverSequentially());
    }

    public void StopVoiceOver()
    {
        StopAllCoroutines();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator PlayVoiceOverSequentially()
    {
        if (voiceClips == null || voiceClips.Length == 0) yield break;

        foreach (AudioClip clip in voiceClips)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
            }
        }
    }
}