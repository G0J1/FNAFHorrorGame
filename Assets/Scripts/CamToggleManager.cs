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
    
    // private static Camera currentActiveCam;
    private bool camsActivated = false;
    private SecCamController secCamController;


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
        if (!camsActivated)
        {
            owner.GetComponentInChildren<TextMeshProUGUI>().text = "Close Cams";
            camsActivated = true;
            secCamCanvas.enabled = true;
            // disable player cam
            player.GetComponentInChildren<Camera>().enabled = false;
            // go to cam01
            cam01.enabled = true;
            SecCamController.SetCurrentActiveCamera(cam01);

        }
        else if (camsActivated) 
        {
            owner.GetComponentInChildren<TextMeshProUGUI>().text = "Open Cams";
            camsActivated = false;
            secCamCanvas.enabled = false;
            // enable player cam
            player.GetComponentInChildren<Camera>().enabled = true;
            // disable current cam
            // currentActiveCam.enabled = false;
           SecCamController.GetCurrentActiveCamera().enabled = false;
        }    
    }
}
