using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    void Start()
    {
        if (Input.anyKey)
        {
            Debug.Log("A key or mouse click has been detected");
        }

    }

    public static event Action OnAnyKeyPressed;
    
    void Update()
    {
        // Check for any key press and invoke the event
        if (Input.anyKeyDown && OnAnyKeyPressed != null)
        {
            OnAnyKeyPressed();
        }
    }

    private static InputManager _instance;

    private void Awake()
    {
        // Making one instance of this script.
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
