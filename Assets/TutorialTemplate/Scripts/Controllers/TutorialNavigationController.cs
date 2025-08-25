
using System.Collections;
using UnityEngine;

public class TutorialNavigationController : MonoBehaviour
{
    [Header("Tutorial Controller Reference")]
    public TutorialControllerOnSequence tutorialController;
    
    [Header("Navigation Settings")]
    public bool enableNavigation = true;
    
    private void Awake()
    {
        if (tutorialController == null)
            tutorialController = FindObjectOfType<TutorialControllerOnSequence>();
    }
    
    public void Next()
    {
        if (!enableNavigation || tutorialController == null) return;
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        
        if (currentSequenceIndex < 0)
        {
            tutorialController.StartTutorial();
            return;
        }
        
        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
        int currentStepIndex = currentSequence.GetCurrentStep();
        
        if (currentStepIndex >= currentSequence.steps.Count - 1)
        {
            if (currentSequenceIndex < tutorialController.sequences.Count - 1)
            {
                tutorialController.StopAllCoroutines();
                tutorialController.NextSequence();
            }
            else
            {
                currentSequence.NextStep();
            }
        }
        else
        {
            currentSequence.NextStep();
        }
    }
    
    public void Previous()
    {
        if (!enableNavigation || tutorialController == null) return;

        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();

        if (currentSequenceIndex < 0) return;

        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
        int currentStepIndex = currentSequence.GetCurrentStep();

        if (currentSequenceIndex == 0 && currentStepIndex == 0)
        {
            Debug.Log("Already at the first step of the first tutorial");
            return;
        }

        if (currentStepIndex == 0)
        {
            tutorialController.StopAllCoroutines();
            tutorialController.GoToPreviousSequenceLastStep();
        }
        else
        {
            currentSequence.PreviousStep();
        }
    }
    
    public void GoToPreviousStep(int stepsBack)
    {
        if (!enableNavigation || tutorialController == null || stepsBack <= 0) return;
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        if (currentSequenceIndex < 0) return;
        
        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
        int currentStepIndex = currentSequence.GetCurrentStep();
        
        int totalCurrentStep = GetTotalStepPosition(currentSequenceIndex, currentStepIndex);
        int targetTotalStep = totalCurrentStep - stepsBack;
        
        if (targetTotalStep < 0)
        {
            Debug.Log($"Cannot go back {stepsBack} steps. Would go beyond the first step.");
            return;
        }
        
        var targetPosition = GetSequenceAndStepFromTotalPosition(targetTotalStep);
        
        if (targetPosition.sequenceIndex == -1)
        {
            Debug.Log("Invalid target position calculated");
            return;
        }
        
        GoToSpecificStep(targetPosition.sequenceIndex, targetPosition.stepIndex);
    }
    
    public void GoToSpecificStep(int sequenceIndex, int stepIndex)
    {
        if (!enableNavigation || tutorialController == null) return;
        
        if (sequenceIndex < 0 || sequenceIndex >= tutorialController.sequences.Count)
        {
            Debug.LogError($"Invalid sequence index: {sequenceIndex}");
            return;
        }
        
        var targetSequence = tutorialController.sequences[sequenceIndex].sequence;
        if (stepIndex < 0 || stepIndex >= targetSequence.steps.Count)
        {
            Debug.LogError($"Invalid step index: {stepIndex} for sequence {sequenceIndex}");
            return;
        }
        
        tutorialController.StopAllCoroutines();
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        if (currentSequenceIndex >= 0 && currentSequenceIndex != sequenceIndex)
        {
            var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
            currentSequence.CloseSequence();
        }
        
        StartCoroutine(NavigateToSpecificStepCoroutine(sequenceIndex, stepIndex));
    }
    
    private IEnumerator NavigateToSpecificStepCoroutine(int sequenceIndex, int stepIndex)
    {
        var targetSequence = tutorialController.sequences[sequenceIndex].sequence;
        
        var controllerType = tutorialController.GetType();
        var currentSequenceIndexField = controllerType.GetField("currentSequenceIndex", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (currentSequenceIndexField != null)
        {
            currentSequenceIndexField.SetValue(tutorialController, sequenceIndex);
        }
        
        targetSequence.useVoiceOver = tutorialController.useVoiceOver;
        targetSequence.gameObject.SetActive(true);
        targetSequence.isRunning = true;
        
        yield return null;
        
        StartCoroutine(targetSequence.GoToStepCoroutine(stepIndex));
    }
    
    private int GetTotalStepPosition(int sequenceIndex, int stepIndex)
    {
        int totalSteps = 0;
        
        for (int i = 0; i < sequenceIndex; i++)
        {
            if (i < tutorialController.sequences.Count)
            {
                totalSteps += tutorialController.sequences[i].sequence.steps.Count;
            }
        }
        
        totalSteps += stepIndex;
        
        return totalSteps;
    }
    
    private (int sequenceIndex, int stepIndex) GetSequenceAndStepFromTotalPosition(int totalPosition)
    {
        int currentTotal = 0;
        
        for (int sequenceIdx = 0; sequenceIdx < tutorialController.sequences.Count; sequenceIdx++)
        {
            int stepsInSequence = tutorialController.sequences[sequenceIdx].sequence.steps.Count;
            
            if (totalPosition < currentTotal + stepsInSequence)
            {
                int stepIdx = totalPosition - currentTotal;
                return (sequenceIdx, stepIdx);
            }
            
            currentTotal += stepsInSequence;
        }
        
        return (-1, -1);
    }
    
    public bool CanGoToPreviousStep(int stepsBack)
    {
        if (!enableNavigation || tutorialController == null || stepsBack <= 0) return false;
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        if (currentSequenceIndex < 0) return false;
        
        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
        int currentStepIndex = currentSequence.GetCurrentStep();
        
        int totalCurrentStep = GetTotalStepPosition(currentSequenceIndex, currentStepIndex);
        return totalCurrentStep >= stepsBack;
    }
    
    public bool CanGoNext()
    {
        if (!enableNavigation || tutorialController == null) return false;
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        
        if (currentSequenceIndex < 0) return true;
        
        if (currentSequenceIndex >= tutorialController.sequences.Count - 1)
        {
            var lastSequence = tutorialController.sequences[currentSequenceIndex].sequence;
            int currentStepIndex = lastSequence.GetCurrentStep();
            
            return currentStepIndex < lastSequence.steps.Count - 1;
        }
        
        return true;
    }
    
    public bool CanGoPrevious()
    {
        if (!enableNavigation || tutorialController == null) return false;
        
        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();
        
        if (currentSequenceIndex < 0) return false;
        
        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;
        int currentStepIndex = currentSequence.GetCurrentStep();
        
        return !(currentSequenceIndex == 0 && currentStepIndex == 0);
    }
    
    public TutorialProgressInfo GetCurrentProgress()
    {
        if (tutorialController == null) return new TutorialProgressInfo();

        int currentSequenceIndex = tutorialController.GetCurrentSequenceIndex();

        if (currentSequenceIndex < 0)
        {
            return new TutorialProgressInfo
            {
                sequenceIndex = -1,
                stepIndex = -1,
                sequenceName = "",
                isRunning = false
            };
        }

        var currentSequence = tutorialController.sequences[currentSequenceIndex].sequence;

        return new TutorialProgressInfo
        {
            sequenceIndex = currentSequenceIndex,
            stepIndex = currentSequence.GetCurrentStep(),
            sequenceName = currentSequence.gameObject.name,
            isRunning = currentSequence.IsRunning()
        };
    }
}

[System.Serializable]
public struct TutorialProgressInfo
{
    public int sequenceIndex;
    public int stepIndex;
    public string sequenceName;
    public bool isRunning;
}