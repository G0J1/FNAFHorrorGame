using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager gameInstance { get; private set; }

    [SerializeField] private Canvas officeUICanvas;
    [SerializeField] private Canvas deathScreenCanvas;
    [SerializeField] private Canvas winScreenCanvas;

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
        deathScreenCanvas.enabled = false;
        winScreenCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerPlayerDeath()
    {
        Invoke(nameof(ShowDeathScreen), 2.0f);
        Invoke(nameof(ReturnToMainMenu), 5.0f);

       
    }

    public void TriggerWinCondition()
    {
        ShowWinScreen();
        Invoke(nameof(ReturnToMainMenu), 5.0f);
    }

    private void ShowDeathScreen()
    {
        officeUICanvas.enabled = false;
        deathScreenCanvas.enabled = true;
    }

    private void ShowWinScreen()
    {
        officeUICanvas.enabled = false;
        winScreenCanvas.enabled = true;
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
