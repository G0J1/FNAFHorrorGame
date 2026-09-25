using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera fppCamera;
    public InputActionAsset inputActions;

    [SerializeField] private float cameraRotationBound = 100.0f;
    [SerializeField] private float cameraRotationSpeed = 1.0f;
    [SerializeField] private bool isCamInCutscene = false;
    [SerializeField] private Animator animController;
    private Quaternion backQuart = Quaternion.Euler(0f, 170f, 0f);
    private Quaternion frontQuart = Quaternion.Euler(0f, 0f, 0f);

    private string tazerEnterAnim = "PlayerTazeStart";
    private string tazerExitAnim = "PlayerTazeEnd";

    [SerializeField] private GameObject tazer;

    private InputAction ia_click;
    private InputAction ia_look;

    private Coroutine activeRotationCoroutine;

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

        HideTazer();



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
        /*fppCamera.transform.localRotation = Quaternion.Euler(0f, -180, 0f);*/
        isCamInCutscene = true;
        Quaternion currentRot = fppCamera.transform.localRotation;
        Quaternion backQuart = Quaternion.Euler(0f, -180.0f, 0f);
        fppCamera.transform.localRotation = Quaternion.RotateTowards(currentRot, backQuart, rotSpeed*Time.deltaTime);

    }
    public void CameraLookFront(float rotSpeed)
    {
        //inputActions.FindActionMap("Player").Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;

        isCamInCutscene = false;

        Quaternion currentRot = fppCamera.transform.localRotation;
        fppCamera.transform.localRotation = Quaternion.RotateTowards(currentRot, frontQuart, rotSpeed);

    }

    public void BeginTaze()
    {
        StartCoroutine(PerformTaze());
    }

    public IEnumerator PerformTaze()
    {
        //CameraLookBehind(50f);
        inputActions.FindActionMap("Player").Disable();
        Cursor.lockState = CursorLockMode.Locked;
        isCamInCutscene = true;
        animController.Play(tazerEnterAnim, 0, 0);
        yield return new WaitForSeconds(1.3f);

        if (activeRotationCoroutine != null) StopCoroutine(activeRotationCoroutine);
        activeRotationCoroutine = StartCoroutine(RotateTo(backQuart, 360.0f));
        ShowTazer();


    }

    public void endTaze()
    {
        /*Cursor.lockState = CursorLockMode.Confined;
        isCamInCutscene = false;*/
        animController.Play(tazerExitAnim, 0, 0);
        if (activeRotationCoroutine != null) StopCoroutine(activeRotationCoroutine);
        activeRotationCoroutine = StartCoroutine(RotateTo(frontQuart, 360.0f));
        

    }


    private IEnumerator RotateTo(Quaternion endRot, float speed)
    {
        float step = speed * Time.deltaTime;
        while (Quaternion.Angle(fppCamera.transform.localRotation, endRot) > 0.0f)
        {
            fppCamera.transform.localRotation =  Quaternion.RotateTowards(fppCamera.transform.localRotation, endRot, step);
            yield return null;
        }

        if (endRot == frontQuart)
        {
            Cursor.lockState = CursorLockMode.Confined;
            inputActions.FindActionMap("Player").Enable();
            isCamInCutscene = false;
            HideTazer();
        }





    }

    private void HideTazer()
    {
        tazer.SetActive(false);
    }

    private void ShowTazer()
    {
        tazer.SetActive(true);
    }
}
