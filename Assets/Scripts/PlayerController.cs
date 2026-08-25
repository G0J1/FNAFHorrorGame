using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    public Camera fppCamera;
    public InputActionAsset inputActions;

    [SerializeField] private float cameraRotationBound = 100.0f;
    [SerializeField] private float cameraRotationSpeed = 1.0f;
    [SerializeField] private bool isCamInCutscene = false;

    private InputAction ia_click;
    private InputAction ia_look;

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

        ia_look = InputSystem.actions.FindActionMap("Player").FindAction("Look");
       /* ia_look.performed += CameraScroll;*/
        ia_look.Enable();



    }

    // Update is called once per frame
    void Update()
    {
        if (!isCamInCutscene)
        {
            Vector2 mousePos = ia_look.ReadValue<Vector2>();
            Debug.Log($"Mouse moved: {mousePos}");
            float xPos = mousePos.x;
            Debug.Log($"X position: {xPos}");
            float scWidth = Screen.width;
            Debug.Log($"Screen width: {scWidth}");
            float halfscWidth = scWidth / 2;
            float adjustedMouseX = xPos - halfscWidth;
            Debug.Log($"Adjusted mouse pos: {adjustedMouseX}");

            if (adjustedMouseX > cameraRotationBound)
            {
                fppCamera.transform.Rotate(new Vector3(0, cameraRotationSpeed * Time.deltaTime, 0));
            }
            else if (adjustedMouseX < -(cameraRotationBound))
            {
                fppCamera.transform.Rotate(new Vector3(0, -(cameraRotationSpeed * Time.deltaTime), 0));
            }
        }
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

   /* private void CameraScroll(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        Debug.Log($"Mouse moved: {delta}");
        float xPos = delta.x;
        Debug.Log($"X position: {xPos}");
        float scWidth = Screen.width;
        Debug.Log($"Screen width: {scWidth}");
        float halfscWidth = scWidth / 2;
        float adjustedMouseX = xPos - halfscWidth;
        Debug.Log($"Adjusted mouse pos: {adjustedMouseX}");

        if (adjustedMouseX > cameraRotationBound )
        {
            fppCamera.transform.Rotate(new Vector3(0, cameraRotationSpeed * Time.deltaTime, 0));
        }
        else if (adjustedMouseX < -cameraRotationBound)
        {
            fppCamera.transform.Rotate(new Vector3(0, -(cameraRotationSpeed * Time.), 0f));
        }
        
    }*/

    public void CameraLookBehind(float rotSpeed)
    {
        inputActions.FindActionMap("Player").Disable();
        Cursor.lockState = CursorLockMode.Locked;
        fppCamera.transform.localRotation = Quaternion.Euler(0f, -180, 0f);
        isCamInCutscene = true;
    }
    public void CameraLookFront(float rotSpeed)
    {
        inputActions.FindActionMap("Player").Enable();
        /*Cursor.lockState = CursorLockMode.None;*/
        Cursor.lockState = CursorLockMode.Confined;
        fppCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        isCamInCutscene = false;
    }
}
