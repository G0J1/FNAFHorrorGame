using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TimeManager timeManagerInstance {  get; private set; }

    [SerializeField] private int hours = 0;
    [SerializeField] private int minutes = 0;
    [SerializeField] private float tickRate = 1.5f;
    [SerializeField] private TextMeshProUGUI timeTextGUI;


    private void Awake()
    {
        if (timeManagerInstance == null)
        {
            timeManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayTime();
        InvokeRepeating(nameof(IncrementTime), tickRate, tickRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IncrementTime()
    {
        minutes += 1;
        if (minutes == 60)
        {
            minutes = 0;
            hours += 1;
        }
        DisplayTime();
        if (hours == 6)
        {
            GameSceneManager.gameInstance.TriggerWinCondition();
        }
    }

    private void DisplayTime()
    {
        int tempHour = 0;
        string tempMins = "";
        if (hours == 0 )
        {
            tempHour = 12;
        }
        else
        {
            tempHour = hours;
        }

        if (minutes < 10)
        {
            tempMins = "0" + minutes.ToString();
        }
        else
        {
            tempMins += minutes.ToString();
        }
        /*timeTextGUI.text = tempHour.ToString() + ":" + tempMins.ToString() + "AM";*/
        timeTextGUI.text = tempHour.ToString() + "AM";
    }
}
