using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialControllerOnSequence : MonoBehaviour
{
    [Header("Settings")]
    public bool Open;

    [Header("Voice Settings")]
    public bool useVoiceOver = true;

    [System.Serializable]
    public class SequenceEntry
    {
        public TutorialSequenceController sequence;
        [Header("Sequence Settings")]
        public bool open = true;
    }

    [Header("Tutorial Sequences")]
    public List<SequenceEntry> sequences = new List<SequenceEntry>();

    [Header("UI")]
    public Text progressText;

    private int currentSequenceIndex = -1;
    private bool isRunning = false;
    private int totalSteps;

    private void Awake()
    {
        foreach (var entry in sequences)
        {
            if (entry.sequence != null)
                entry.sequence.gameObject.SetActive(false);
        }

        totalSteps = 0;
        for (int i = 0; i < sequences.Count; i++)
        {
            if (sequences[i].sequence != null)
            {
                sequences[i].sequence.parentController = this;
                sequences[i].sequence.sequenceIndex = i;
                
                if (sequences[i].open)
                {
                    totalSteps += sequences[i].sequence.steps.Count;
                }
            }
        }
    }

    private void Start()
    {
        // Only auto-start if Open is true AND we're not controlled by a module selector
        TutorialModuleSelector moduleSelector = FindObjectOfType<TutorialModuleSelector>();
        if (Open && (moduleSelector == null || moduleSelector.ShouldAutoStart()))
        {
            StartTutorial();
        }
    }

    private void OnEnable()
    {
        // Don't auto-start on enable - let the module selector control this
        // This prevents unwanted auto-starts when returning from main menu
    }

    public void StartTutorial()
    {
        // Don't start if not open or already running
        if (!Open || isRunning || sequences.Count == 0) return;

        int firstOpenSequence = GetFirstOpenSequenceIndex();
        if (firstOpenSequence == -1)
        {
            Debug.LogWarning("No open tutorial sequences found!");
            return;
        }

        isRunning = true;
        StartCoroutine(RunSequence(firstOpenSequence));
    }

    public void NextSequence()
    {
        int nextOpenSequence = GetNextOpenSequenceIndex(currentSequenceIndex);
        if (nextOpenSequence != -1)
        {
            StartCoroutine(RunSequence(nextOpenSequence));
        }
        else
        {
            isRunning = false;
        }
    }

    private IEnumerator RunSequence(int index)
    {
        if (currentSequenceIndex >= 0 && currentSequenceIndex < sequences.Count)
        {
            var current = sequences[currentSequenceIndex];
            current.sequence.CloseSequence();
        }

        if (index >= sequences.Count || !sequences[index].open)
        {
            isRunning = false;
            yield break;
        }

        currentSequenceIndex = index;
        var next = sequences[index];
        next.sequence.useVoiceOver = useVoiceOver;

        next.sequence.gameObject.SetActive(true);
        next.sequence.OpenSequence();

        while (next.sequence.IsRunning())
            yield return null;

        yield return new WaitForSeconds(0.5f);
        NextSequence();
    }

    public void ResetTutorial()
    {
        StopAllCoroutines();

        // Stop all voice overs before resetting
        foreach (var entry in sequences)
        {
            if (entry.sequence != null)
            {
                // Stop voice overs in all steps
                foreach (var stepEntry in entry.sequence.steps)
                {
                    if (stepEntry.stepScript != null)
                    {
                        stepEntry.stepScript.StopVoiceOver();
                    }
                }
                
                // Close the sequence properly
                entry.sequence.CloseSequence();
                entry.sequence.gameObject.SetActive(false);
            }
        }

        currentSequenceIndex = -1;
        isRunning = false;

        if (progressText != null)
            progressText.text = "0/0";
    }

    public void UpdateProgress(int sequenceIndex, int stepIndex)
    {
        int currentStep = 0;
        
        for (int i = 0; i < sequenceIndex; i++)
        {
            if (sequences[i].open)
                currentStep += sequences[i].sequence.steps.Count;
        }

        if (sequences[sequenceIndex].open)
            currentStep += stepIndex + 1;

        if (progressText != null)
            progressText.text = $"{currentStep}/{totalSteps}";
    }

    public void GoToPreviousSequenceLastStep()
    {
        StopAllCoroutines();

        int prevIndex = GetPreviousOpenSequenceIndex(currentSequenceIndex);
        if (prevIndex < 0) return;

        sequences[currentSequenceIndex].sequence.CloseSequence();

        currentSequenceIndex = prevIndex;
        var sequence = sequences[currentSequenceIndex].sequence;

        sequence.useVoiceOver = useVoiceOver;
        sequence.gameObject.SetActive(true);

        int lastIndex = sequence.steps.Count - 1;
        sequence.isRunning = true;
        sequence.StartCoroutine(sequence.GoToStepCoroutine(lastIndex));
    }

    private int GetFirstOpenSequenceIndex()
    {
        for (int i = 0; i < sequences.Count; i++)
        {
            if (sequences[i].open && sequences[i].sequence != null)
                return i;
        }
        return -1;
    }

    private int GetNextOpenSequenceIndex(int currentIndex)
    {
        for (int i = currentIndex + 1; i < sequences.Count; i++)
        {
            if (sequences[i].open && sequences[i].sequence != null)
                return i;
        }
        return -1;
    }

    private int GetPreviousOpenSequenceIndex(int currentIndex)
    {
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (sequences[i].open && sequences[i].sequence != null)
                return i;
        }
        return -1;
    }

    public void SetSequenceOpen(int sequenceIndex, bool isOpen)
    {
        if (sequenceIndex >= 0 && sequenceIndex < sequences.Count)
        {
            bool wasOpen = sequences[sequenceIndex].open;
            sequences[sequenceIndex].open = isOpen;
            
            if (wasOpen != isOpen)
            {
                RecalculateTotalSteps();
            }
            
            Debug.Log($"Sequence {sequenceIndex} open state changed to: {isOpen}");
        }
    }

    public bool IsSequenceOpen(int sequenceIndex)
    {
        if (sequenceIndex >= 0 && sequenceIndex < sequences.Count)
            return sequences[sequenceIndex].open;
        return false;
    }

    public List<int> GetOpenSequenceIndices()
    {
        List<int> openIndices = new List<int>();
        for (int i = 0; i < sequences.Count; i++)
        {
            if (sequences[i].open && sequences[i].sequence != null)
                openIndices.Add(i);
        }
        return openIndices;
    }

    private void RecalculateTotalSteps()
    {
        totalSteps = 0;
        for (int i = 0; i < sequences.Count; i++)
        {
            if (sequences[i].sequence != null && sequences[i].open)
            {
                totalSteps += sequences[i].sequence.steps.Count;
            }
        }
        
        if (currentSequenceIndex >= 0 && currentSequenceIndex < sequences.Count)
        {
            var currentSequence = sequences[currentSequenceIndex].sequence;
            int currentStepIndex = currentSequence.GetCurrentStep();
            UpdateProgress(currentSequenceIndex, currentStepIndex);
        }
    }

    public int GetCurrentSequenceIndex() => currentSequenceIndex;
}