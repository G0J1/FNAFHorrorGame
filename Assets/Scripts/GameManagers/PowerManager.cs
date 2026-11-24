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

    [Header("Audio")]
    [SerializeField] private AudioClip powerOutAudio;


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
        UpdatePowerTextGUI();
        InvokeRepeating(nameof(PassiveDrain), 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PassiveDrain()
    {
        if (currentPower > 0)
        {
            DrainPower(passiveDrainRate);
            UpdatePowerTextGUI();
        }
        
    }

    private void CamDrain()
    {
        DrainPower(camDrainRate);
        UpdatePowerTextGUI();
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
            UpdatePowerTextGUI();
        }
        else if (currentPower < 0)
        {
            CamToggleManager.camToggleManagerInstance.CloseCams();
            OnPowerDown();
        }
    }

    public void AddPower(float toAdd)
    {
        if (currentPower + toAdd < maxPower)
        {
            currentPower += toAdd;
            UpdatePowerTextGUI();
            if (!(SoundManager.gameInstance.fanActive))
            {
                SoundManager.gameInstance.EnableFan();
            }
        }

       
    }

    public float GetCurrentPower()
    {
        return currentPower;
    }

    public void SetPower(float amount)
    {
        currentPower = amount;
        if (amount == 0)
        {
            OnPowerDown();
        }
        else if (amount > 0)
        {
            if (!(SoundManager.gameInstance.fanActive))
            {
                SoundManager.gameInstance.EnableFan();
            }
        }
    }


    private void OnPowerDown()
    {
        UpdatePowerTextGUI();
        if (powerOutAudio != null)
        {
            SoundManager.gameInstance.PlaySound(powerOutAudio);
            SoundManager.gameInstance.DisableFan();
        }
    }

    private void UpdatePowerTextGUI()
    {
        powerTextGUI.text = "Power left: " + PowerToString() + "%";
    }

    private string PowerToString()
    {
        int powerInt = (int)currentPower;
        if (powerInt < 0)
        {
            powerInt = 0;
        }
        return powerInt.ToString();
    }


}
