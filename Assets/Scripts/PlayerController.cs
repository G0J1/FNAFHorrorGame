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

        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        ia_click = InputSystem.actions.FindActionMap("Player").FindAction("Click");
        ia_click.started += OnClickStart;
        ia_click.canceled += OnClickEnd;
        ia_click.Enable();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleClickTrace()
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.value;
        Ray rayOrign = fppCamera.ScreenPointToRay(mousePos);
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

    private void OnClickStart(InputAction.CallbackContext context)
    {
        Debug.Log("Click!");
        InvokeRepeating(nameof(HandleClickTrace), 0.0f, 0.5f);

    }

    private void OnClickEnd(InputAction.CallbackContext context)
    {
        CancelInvoke(nameof(HandleClickTrace));
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
        /*Cursor.lockState = CursorLockMode.None;*/
        Cursor.lockState = CursorLockMode.Confined;
        fppCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
