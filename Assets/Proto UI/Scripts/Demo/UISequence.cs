using UnityEngine;

public class UISequence : MonoBehaviour
{
    public void ToLogin()
    {
        UIStateManager.Instance.ChangeState(UIState.Login);
    }

    public void ToMenuHor()
    {
        UIStateManager.Instance.ChangeState(UIState.MainMenuHor);
    }

    public void ToMenuVer()
    {
        UIStateManager.Instance.ChangeState(UIState.MainMenuVer);
    }

    public void ToVideoPlayer()
    {
        UIStateManager.Instance.ChangeState(UIState.VideoPlayer);
    }

    public void ToQuizNonPic()
    {
        UIStateManager.Instance.ChangeState(UIState.QuizWindowNonPic);
    }

    public void ToQuizWPic()
    {
        UIStateManager.Instance.ChangeState(UIState.QuizWindowPic);
    }
}
