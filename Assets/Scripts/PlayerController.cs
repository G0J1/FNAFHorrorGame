using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera fppCamera;
    public InputActionAsset inputActions;
    public CinemachineCamera cmCamera;
   

    private GameObject[] securityCams;
    // private int camIndex = 0;

    // private InputAction ia_camUp;
    private InputAction ia_click;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fppCamera.enabled = true;

        Cursor.visible = true;

        //securityCams = GameObject.FindGameObjectsWithTag("Cam");
        //Debug.Log(securityCams.Length);

        //ia_camUp = InputSystem.actions.FindActionMap("Player").FindAction("CamUp");
        //ia_camUp.performed += CamUp;
        //ia_camUp.Enable();

        ia_click = InputSystem.actions.FindActionMap("Player").FindAction("Click");
        ia_click.performed += Click;
        ia_click.Enable();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void CamUp(InputAction.CallbackContext context)
    //{
    //    Debug.Log("increasing cam active!");
    //    fppCamera.enabled = false;
    //    if (securityCams[camIndex] != null)
    //    {
    //        if (camIndex > 0)
    //        {
    //            securityCams[camIndex - 1].GetComponent<Camera>().enabled = false;
    //        }
    //        securityCams[camIndex].GetComponent<Camera>().enabled = true;
    //    }
        

    //    camIndex++;
    //}

    private void Click(InputAction.CallbackContext context)
    {
        Debug.Log("Click!");
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.value;
        Ray rayOrign = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(rayOrign, out hit))
        {
            string hitObject = hit.collider.gameObject.name;
            Debug.Log("Hit object: " + hitObject);
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }

    }

    public void CameraLookBehind(float rotSpeed)
    {
        cmCamera.enabled = false;
        inputActions.FindActionMap("Player").Disable();
        Cursor.lockState = CursorLockMode.Locked;
        fppCamera.transform.localRotation = Quaternion.Euler(0f, -145.08f, 0f);
    }
    public void CameraLookFront(float rotSpeed)
    {
        cmCamera.enabled = true;
        inputActions.FindActionMap("Player").Enable();
        Cursor.lockState = CursorLockMode.None;
        fppCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
