using UnityEngine;
using UnityEngine.EventSystems;

public class VideoInteractView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject m_playPauseButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowComponents();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideComponents();
    }

    private void ShowComponents()
    {
        m_playPauseButton.SetActive(true);
    }

    private void HideComponents()
    {
        m_playPauseButton.SetActive(false);
    }
}
