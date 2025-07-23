using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequenceController : MonoBehaviour
{
    [System.Serializable]
    public class StepEntry
    {
        public SequenceStep stepScript;
        public UnityEvent onOpen;
        public UnityEvent onClose;
        public float delayAfterFinish = 0f; // Delay before next step
    }

    [Header("Settings")]
    [SerializeField] private float defaultStepDelay = 0f;

    [Header("Steps")]
    public List<StepEntry> steps = new List<StepEntry>();

    [HideInInspector] public bool useVoiceOver = true;

    private int currentStep = -1;
    private bool isRunning = false;

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

    private IEnumerator GoToStepCoroutine(int index)
    {
        if (currentStep >= 0 && currentStep < steps.Count)
        {
            StepEntry current = steps[currentStep];
            if (current.stepScript != null)
                current.stepScript.gameObject.SetActive(false);

            current.onClose?.Invoke();

            float delay = current.delayAfterFinish >= 0 ? current.delayAfterFinish : defaultStepDelay;
            yield return new WaitForSeconds(delay);
        }

        if (index >= steps.Count)
        {
            CloseSequence();
            yield break;
        }

        StepEntry next = steps[index];
        if (next.stepScript != null)
            next.stepScript.gameObject.SetActive(true);

        next.onOpen?.Invoke();

        if (useVoiceOver && next.stepScript != null)
        {
            next.stepScript.PlayVoiceOver();
        }

        currentStep = index;
    }

    public void CloseSequence()
    {
        if (!isRunning) return;

        if (currentStep >= 0 && currentStep < steps.Count)
        {
            var step = steps[currentStep];
            if (step.stepScript != null)
                step.stepScript.gameObject.SetActive(false);

            step.onClose?.Invoke();
        }

        currentStep = -1;
        isRunning = false;

        gameObject.SetActive(false);
    }

    public int GetCurrentStep() => currentStep;

    public bool IsRunning() => isRunning;
}
