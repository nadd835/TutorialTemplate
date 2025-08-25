using UnityEngine;

public class TutorialModuleSelector : MonoBehaviour
{
    [Header("Tutorial Modules")]
    public TutorialControllerOnSequence[] modules;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;

    [Header("Voice Over")]
    public bool forceVoiceOver = false;

    private TutorialControllerOnSequence currentModule;
    private bool shouldAutoStart = true;

    public void SelectModule(TutorialControllerOnSequence moduleToActivate)
    {
        if (moduleToActivate != null && !moduleToActivate.Open)
        {
            Debug.LogWarning($"Module {moduleToActivate.name} is not set to Open. Cannot start tutorial.");
            return;
        }

        shouldAutoStart = true;

        foreach (var module in modules)
        {
            if (module == null) continue;

            if (module == moduleToActivate)
            {
                module.gameObject.SetActive(true);

                if (forceVoiceOver)
                {
                    module.useVoiceOver = true;
                }

                currentModule = module;

                // Reset and start the tutorial properly
                if (shouldAutoStart && module.Open)
                {
                    module.ResetTutorial();
                    // Start coroutine on the active module instead of this (potentially inactive) gameobject
                    module.StartCoroutine(StartTutorialAfterReset(module));
                }
            }
            else
            {
                module.gameObject.SetActive(false);
                module.ResetTutorial();
            }
        }

        // Deactivate main menu panel after setting up the module
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    private System.Collections.IEnumerator StartTutorialAfterReset(TutorialControllerOnSequence module)
    {
        yield return null; // Wait one frame
        if (module != null && module.Open)
        {
            module.StartTutorial();
        }
    }

    public void StartCurrentModuleTutorial()
    {
        if (currentModule != null && currentModule.Open)
        {
            currentModule.StartTutorial();
        }
        else if (currentModule != null && !currentModule.Open)
        {
            Debug.LogWarning($"Cannot start tutorial for {currentModule.name}: Module is not set to Open");
        }
    }

    public void ResetAllModules()
    {
        foreach (var module in modules)
        {
            if (module != null)
            {
                module.ResetTutorial();
                module.gameObject.SetActive(false);
            }
        }

        currentModule = null;
        shouldAutoStart = true;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void CloseCurrentModuleAndOpenMenu()
    {
        if (currentModule != null)
        {
            shouldAutoStart = false;

            // Stop all voice overs before closing
            StopAllVoiceOvers(currentModule);
            
            currentModule.ResetTutorial();
            currentModule.gameObject.SetActive(false);
            currentModule = null;
        }

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    private void StopAllVoiceOvers(TutorialControllerOnSequence module)
    {
        if (module == null) return;

        // Stop voice overs in all sequences
        foreach (var sequenceEntry in module.sequences)
        {
            if (sequenceEntry.sequence != null)
            {
                // Stop voice overs in all steps of the sequence
                foreach (var stepEntry in sequenceEntry.sequence.steps)
                {
                    if (stepEntry.stepScript != null)
                    {
                        stepEntry.stepScript.StopVoiceOver();
                    }
                }
            }
        }
    }

    public void EnableModuleAutoStart()
    {
        shouldAutoStart = true;
    }

    public bool CanOpenModule(TutorialControllerOnSequence module)
    {
        return module != null && module.Open;
    }

    public void SetModuleOpenState(TutorialControllerOnSequence module, bool isOpen)
    {
        if (module != null)
        {
            module.Open = isOpen;
            Debug.Log($"Module {module.name} Open state set to: {isOpen}");
        }
    }

    public TutorialControllerOnSequence GetActiveModule() => currentModule;

    public bool ShouldAutoStart() => shouldAutoStart;
}