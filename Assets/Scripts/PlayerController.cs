using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public Camera camera;
    //public InputActionAsset inputActions;

    //private InputAction m_look;

    //private void OnEnable()
    //{
    //    inputActions.FindActionMap("Player").Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.FindActionMap("Player").Disable();
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_look = InputSystem.actions.FindAction("Look");
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
