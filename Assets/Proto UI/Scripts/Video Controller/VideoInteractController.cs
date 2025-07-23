using UnityEngine;
using UnityEngine.Video;

public class VideoInteractController : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer m_videoPlayer;

    private void OnEnable()
    {
        UIActions.OnPlayPauseButtonPressed += OnPlayPauseButtonPressed;
    }

    private void OnDisable()
    {
        UIActions.OnPlayPauseButtonPressed -= OnPlayPauseButtonPressed;
    }

    private void OnPlayPauseButtonPressed(bool playState)
    {
        switch (playState)
        {
            case true:
                m_videoPlayer.Pause();
                break;
            case false:
                m_videoPlayer.Play();
                break;
        }
    }
}
