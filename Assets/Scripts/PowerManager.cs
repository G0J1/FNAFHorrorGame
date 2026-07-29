using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private float maxPower = 99.99f;
    [SerializeField] private float currentPower = 99.99f;
    [SerializeField] private float passiveDrainRate = 2.0f;
    [SerializeField] private float camDrainRate = 2.0f;
    [SerializeField] private TextMeshProUGUI powerTextGUI;


    public static PowerManager gameInstance {  get; private set; }

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePowerTextGUI(currentPower);
        InvokeRepeating(nameof(PassiveDrain), 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PassiveDrain()
    {
        DrainPower(passiveDrainRate);
        UpdatePowerTextGUI(currentPower);
    }

    private void CamDrain()
    {
        DrainPower(camDrainRate);
        UpdatePowerTextGUI(currentPower);
    }

    public void BeginCamDrain()
    {
        InvokeRepeating(nameof(CamDrain), 1.0f, 1.0f);
    }

    public void CancelCamDrain()
    {
        CancelInvoke(nameof(CamDrain));
    }

    public void DrainPower(float toDrain)
    {
        if (currentPower > 0)
        {
            currentPower -= toDrain;
            UpdatePowerTextGUI(currentPower);
        }
        else if (currentPower < 0)
        {
            CamToggleManager.camToggleManagerInstance.CloseCams();
        }
    }

    public void AddPower(float toAdd)
    {
        if (currentPower + toAdd < maxPower)
        {
            currentPower += toAdd;
            UpdatePowerTextGUI(currentPower);
        }
       
    }

    public float GetCurrentPower()
    {
        return currentPower;
    }


    private void UpdatePowerTextGUI(float updatedPower)
    {
        powerTextGUI.text = "Power left: " + updatedPower.ToString() + "%";
    }


}
