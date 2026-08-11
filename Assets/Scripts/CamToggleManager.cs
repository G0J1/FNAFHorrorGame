using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CamToggleManager : MonoBehaviour, IPointerEnterHandler
{
    public Button owner;
    public Canvas secCamCanvas;
    public GameObject player;
    public Camera cam01;

    public static CamToggleManager camToggleManagerInstance { get; private set; }
    
    // private static Camera currentActiveCam;
    private bool camsActivated = false;
    private SecCamController secCamController;

    void Awake()
    {
        if (camToggleManagerInstance == null)
        {
            camToggleManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secCamCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!camsActivated && PowerManager.gameInstance.GetCurrentPower() > 0.0f)
        {
            owner.GetComponentInChildren<TextMeshProUGUI>().text = "Close Cams";
            camsActivated = true;
            secCamCanvas.enabled = true;
            // disable player cam
            player.GetComponentInChildren<Camera>().enabled = false;
            // go to cam01
            cam01.enabled = true;
            SecCamController.SetCurrentActiveCamera(cam01);
            PowerManager.gameInstance.BeginCamDrain();

        }
        else if (camsActivated) 
        {
            CloseCams();
        }    
    }

    public void CloseCams()
    {
        if (SecCamController.GetCurrentActiveCamera() != null)
        {
            owner.GetComponentInChildren<TextMeshProUGUI>().text = "Open Cams";
            camsActivated = false;
            secCamCanvas.enabled = false;
            // enable player cam
            player.GetComponentInChildren<Camera>().enabled = true;
            // disable current cam
            // currentActiveCam.enabled = false;
            SecCamController.GetCurrentActiveCamera().enabled = false;
            PowerManager.gameInstance.CancelCamDrain();
        }
        
    }
}
