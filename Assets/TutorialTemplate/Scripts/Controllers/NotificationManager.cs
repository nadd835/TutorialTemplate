using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    private bool isOpen = false;

    public void OpenNotification(string message)
    {
        if (isOpen || notificationPanel == null || notificationText == null)
            return;

        notificationText.text = message;
        notificationPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseNotification()
    {
        if (!isOpen || notificationPanel == null)
            return;

        notificationPanel.SetActive(false);
        isOpen = false;
    }

    public bool IsOpen => isOpen;
}
