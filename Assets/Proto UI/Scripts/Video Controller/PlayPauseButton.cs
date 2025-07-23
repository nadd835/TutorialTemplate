using System;
using UnityEngine;

public class PlayPauseButton : MonoBehaviour
{
    private bool m_playState;
    /// <summary>
    /// m_playState means "is the button at play state?"
    /// </summary>
    /// <returns>True if button is at play state, otherwise False</returns>
    public bool PlayState { get { return m_playState; } set { m_playState = value; } }

    [SerializeField]
    private GameObject m_playIcon;
    [SerializeField]
    private GameObject m_pauseIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(m_playIcon == null || m_pauseIcon == null)
        {
            m_playIcon = transform.Find("PlayIcon").gameObject;
            m_pauseIcon = transform.Find("PauseIcon").gameObject;
        }
    }

    public void ButtonPressed()
    {
        PlayState = !PlayState;

        UpdateIcon();
        UIActions.OnPlayPauseButtonPressed?.Invoke(PlayState);
    }

    private void UpdateIcon()
    {
        switch(PlayState)
        {
            case true:
                m_pauseIcon.SetActive(false);
                m_playIcon.SetActive(true);
                break;
            case false:
                m_playIcon.SetActive(false);
                m_pauseIcon.SetActive(true);
                break;

        }
    }
}
