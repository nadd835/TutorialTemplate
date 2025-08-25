using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSequenceController : MonoBehaviour
{
    [System.Serializable]
    public class StepEntry
    {
        public TutorialSequenceStep stepScript;
        public UnityEvent onOpen;
        public UnityEvent onClose;
        public float delayAfterFinish = 0f;
    }

    [Header("Settings")]
    [SerializeField] private float defaultStepDelay = 0f;

    [Header("Steps")]
    public List<StepEntry> steps = new List<StepEntry>();

    [HideInInspector] public bool useVoiceOver = true;

    // Set by parent controller:
    [HideInInspector] public TutorialControllerOnSequence parentController;
    [HideInInspector] public int sequenceIndex;

    [HideInInspector] public bool isRunning = false;

    private int currentStep = -1;

    public void OpenSequence()
    {
        if (isRunning) return;
        isRunning = true;

        foreach (var step in steps)
        {
            if (step.stepScript != null)
                step.stepScript.gameObject.SetActive(false);
        }

        StartCoroutine(GoToStepCoroutine(0));
    }

    public void NextStep()
    {
        if (!isRunning) return;
        StartCoroutine(GoToStepCoroutine(currentStep + 1));
    }

    public void PreviousStep()
    {
        if (!isRunning) return;

        // Stop voice over from current step before moving
        if (currentStep >= 0 && currentStep < steps.Count)
        {
            var currentStepScript = steps[currentStep].stepScript;
            if (currentStepScript != null)
            {
                currentStepScript.StopVoiceOver();
            }
        }

        int prevIndex = currentStep - 1;
        if (prevIndex < 0)
        {
            parentController?.GoToPreviousSequenceLastStep();
            return;
        }

        StartCoroutine(GoToStepCoroutine(prevIndex));
    }

    public IEnumerator GoToStepCoroutine(int index)
    {
        // Stop voice over and close current step
        if (currentStep >= 0 && currentStep < steps.Count)
        {
            var current = steps[currentStep];

            // Stop voice over before closing the step
            if (current.stepScript != null)
            {
                current.stepScript.StopVoiceOver();
                current.stepScript.gameObject.SetActive(false);
            }

            current.onClose?.Invoke();

            float delay = current.delayAfterFinish >= 0 ? current.delayAfterFinish : defaultStepDelay;
            yield return new WaitForSeconds(delay);
        }

        // Finished sequence
        if (index >= steps.Count)
        {
            CloseSequence();
            yield break;
        }

        // Activate step
        var next = steps[index];
        if (next.stepScript != null)
            next.stepScript.gameObject.SetActive(true);

        next.onOpen?.Invoke();

        parentController?.UpdateProgress(sequenceIndex, index);

        currentStep = index;

        if (useVoiceOver && next.stepScript != null)
            next.stepScript.PlayVoiceOver();
    }

    public void CloseSequence()
    {
        if (!isRunning) return;

        StopAllCoroutines();

        if (currentStep >= 0 && currentStep < steps.Count)
        {
            var step = steps[currentStep];
            if (step.stepScript != null)
            {
                step.stepScript.StopVoiceOver();
                step.stepScript.gameObject.SetActive(false);
            }

            step.onClose?.Invoke();
        }

        foreach (var step in steps)
        {
            if (step.stepScript != null)
            {
                step.stepScript.StopVoiceOver();
                step.stepScript.gameObject.SetActive(false);
            }
        }

        currentStep = -1;
        isRunning = false;

        gameObject.SetActive(false);
    }

    public int GetCurrentStep() => currentStep;
    public bool IsRunning() => isRunning;
}