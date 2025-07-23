using TMPro;
using UnityEngine;

/// <summary>
/// Stores data about the tooltip object and a place to execute methods for the object.
/// </summary>
public class TooltipObject : MonoBehaviour
{
    [SerializeField]
    public TMP_Text m_text;
    [SerializeField]
    private Animator m_anim;

    private Camera m_mainCam;

    private void OnEnable()
    {
        if(!m_anim)
            m_anim = GetComponent<Animator>();

        m_anim.Play("frame_Appear");
    }

    private void Start()
    {
        m_mainCam = Camera.main;
        m_anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.LookAt(m_mainCam.transform);
        transform.Rotate(0, 180, 0);
    }
}
