using UnityEngine;

/// <summary>
/// Tooltip spawner. Requires main cam and tooltip object.
/// </summary>
public class TooltipSpawner : MonoBehaviour
{
    [SerializeField]
    private TooltipObject m_tooltipObject;
    private GameObject m_mainCamera;

    private Vector3 m_startPos;

    private void Start()
    {
        m_mainCamera = Camera.main.gameObject;
        m_startPos = transform.position;
    }

    public void ShowTooltip(GameObject obj)
    {
        m_tooltipObject.m_text.text = obj.name;

        Vector3 toCamera = (m_mainCamera.transform.position - obj.transform.position).normalized;

        Vector3 forwardOffset = toCamera * 0.05f; // Reduce distance
        Vector3 upOffset = Vector3.up * 0.05f;

        m_tooltipObject.transform.position = obj.transform.position + forwardOffset + upOffset;

        m_tooltipObject.enabled = true;
    }

    public void HideTooltip()
    {
        m_tooltipObject.enabled = false;
        m_tooltipObject.transform.position = m_startPos;
    }
}
