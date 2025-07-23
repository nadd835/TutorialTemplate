using UnityEngine;
using System.Collections.Generic;

public enum UIState
{
    Login,
    MainMenuHor,
    MainMenuVer,
    VideoPlayer,
    QuizWindowPic,
    QuizWindowNonPic
}

public class UIStateManager : MonoBehaviour
{
    [System.Serializable]
    public class UIStatePanel
    {
        public UIState state;
        public GameObject panel;
    }

    [Header("Assign each UI State with its panel")]
    public List<UIStatePanel> panels = new List<UIStatePanel>();

    private Dictionary<UIState, GameObject> stateToPanel;
    private UIState currentState;

    public static UIStateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        stateToPanel = new Dictionary<UIState, GameObject>();

        foreach (var item in panels)
        {
            if (!stateToPanel.ContainsKey(item.state))
            {
                stateToPanel.Add(item.state, item.panel);
            }
        }

        currentState = (UIState)(-1);

        // Initialize with first state
        if (panels.Count > 0)
        {
            ChangeState(panels[0].state);
            Debug.Log("INITTTTTT");
        }
    }

    public void ChangeState(UIState newState)
    {
        if (newState == currentState) return;

        foreach (var kvp in stateToPanel)
        {
            kvp.Value.SetActive(kvp.Key == newState);
            Debug.Log($"Setting {kvp.Key} panel active: {kvp.Key == newState}");
        }

        currentState = newState;
    }

    public UIState GetCurrentState()
    {
        return currentState;
    }
}
