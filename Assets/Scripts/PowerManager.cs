using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private float maxPower = 99.99f;
    [SerializeField] private float currentPower = 99.99f;
    [SerializeField] private float passiveDrainRate = 2.0f;
    [SerializeField] private TextMeshProUGUI powerTextGUI;

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

    public void PassiveDrain()
    {
        DrainPower(passiveDrainRate);
        UpdatePowerTextGUI(currentPower);
    }

    public void DrainPower(float toDrain)
    {
        if (currentPower > 0)
        {
            currentPower -= toDrain;
            UpdatePowerTextGUI(currentPower);
        }
    }

    public void AddPower(float toAdd)
    {
        if (currentPower < maxPower)
        {
            currentPower += toAdd;
            UpdatePowerTextGUI(currentPower);
        }
       
    }

    public void UpdatePowerTextGUI(float updatedPower)
    {
        powerTextGUI.text = "Power left: " + updatedPower.ToString() + "%";
    }
}
