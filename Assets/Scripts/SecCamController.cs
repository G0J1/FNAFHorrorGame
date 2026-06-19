using UnityEngine;

public class SecCamController : MonoBehaviour
{
    public Camera secCam;
    public GameObject player;
    

    private static Camera currentActiveCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCamClicked()
    {

        if (currentActiveCam != null) 
        {
            currentActiveCam.enabled = false;
        }
        secCam.enabled = true;
        currentActiveCam = secCam;
    }

    public static Camera GetCurrentActiveCamera() { return currentActiveCam; }

    public static void SetCurrentActiveCamera(Camera secCam) {  currentActiveCam = secCam; }
}
