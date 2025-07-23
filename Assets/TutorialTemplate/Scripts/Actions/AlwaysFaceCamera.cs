using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    public Transform cam;

    private void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (cam == null)
        {
            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }
            return;
        }

        transform.LookAt(transform.position + cam.forward);
    }
}