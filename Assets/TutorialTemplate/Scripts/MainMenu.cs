using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class MainMenu : MonoBehaviour
{
    public GameObject menuObject;
    private InputBridge inputBridge;

    void Start()
    {
        // Get reference to the InputBridge instance
        inputBridge = InputBridge.Instance;
        menuObject.SetActive(false);
    }

    void Update()
    {
        // Check for Y button press (left controller)
        if (inputBridge && inputBridge.BButtonDown)
        {
            menuObject.SetActive(true);
            Debug.Log("Open main menu");
        }
    }
}
