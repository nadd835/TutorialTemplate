using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Settings")]
    public bool autoStart = true;

    [Header("Voice Settings")]
    public bool useVoiceOver = true;

    [System.Serializable]
    public class SequenceEntry
    {
        public SequenceController sequence;
    }

    [Header("Tutorial Sequences")]
    public List<SequenceEntry> sequences = new List<SequenceEntry>();

    private int currentSequenceIndex = -1;
    private bool isRunning = false;

    private void Start()
    {
        // Disable all sequences at the start
        foreach (var entry in sequences)
        {
            if (entry.sequence != null)
                entry.sequence.gameObject.SetActive(false);
        }

        if (autoStart)
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        if (isRunning || sequences.Count == 0) return;

        isRunning = true;
        StartCoroutine(RunSequence(0));
    }

    public void NextSequence()
    {
        StartCoroutine(RunSequence(currentSequenceIndex + 1));
    }

    private IEnumerator RunSequence(int index)
    {
        if (currentSequenceIndex >= 0 && currentSequenceIndex < sequences.Count)
        {
            var current = sequences[currentSequenceIndex];
            current.sequence.CloseSequence();
        }

        if (index >= sequences.Count)
        {
            isRunning = false;
            yield break;
        }

        currentSequenceIndex = index;
        var next = sequences[index];

        next.sequence.useVoiceOver = useVoiceOver;

        next.sequence.gameObject.SetActive(true);
        next.sequence.OpenSequence();

        // Wait until the sequence is finished
        while (next.sequence.IsRunning())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        NextSequence();
    }

    public void ResetTutorial()
    {
        StopAllCoroutines();

        foreach (var entry in sequences)
        {
            if (entry.sequence != null)
                entry.sequence.gameObject.SetActive(false);
        }

        currentSequenceIndex = -1;
        isRunning = false;
    }

    public int GetCurrentSequenceIndex() => currentSequenceIndex;
}
