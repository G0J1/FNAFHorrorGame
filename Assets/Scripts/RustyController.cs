using UnityEngine;
using UnityEngine.UIElements;

public class RustyController : MonoBehaviour
{
    public enum RustyPhase
    {
        MovementPhase,
        AttackPhase
    }

    public Location currentLocation;
    public RustyPhase currentPhase;

    [SerializeField] private GameObject startingPos;
    [SerializeField] private int aILevel = 0;
    [SerializeField] private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(IncrementAILevel), 20.0f, 20.0f);
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetTazed()
    {
        ResetPosition();
        CancelInvoke(nameof(Jumpscare));
    }

    public void ResetPosition()
    {
        currentPhase = RustyPhase.MovementPhase;
        currentLocation = startingPos.GetComponent<Location>();
        gameObject.transform.position = startingPos.transform.position;
        Invoke(nameof(StartNextMovementTimer), 10.0f);
    }

    private void StartNextMovementTimer()
    {
        if (currentPhase == RustyPhase.AttackPhase) { return; }

        float randomTime = GenerateRandomTime();
        Invoke(nameof(BeginMovementAction), randomTime);
        currentPhase = RustyPhase.MovementPhase;
    }

    private void DisableMovement()
    {
        CancelInvoke(nameof(BeginMovementAction));
    }



    private void IncrementLocation()
    {
        Location[] possibleLocations = currentLocation.GetComponent<Location>().nextLocations;
        Location nextLocation = currentLocation;

        if (possibleLocations.Length == 1)
        {
            nextLocation = currentLocation.GetComponent<Location>().nextLocations[0];
        }
        else if (possibleLocations.Length > 1)
        {
            nextLocation = possibleLocations[ChooseRandomLocation(possibleLocations.Length)];
        }

        if (nextLocation != null)
        {
            gameObject.transform.position = nextLocation.transform.position;
            currentLocation = nextLocation;
        }

        if (currentLocation.CompareTag("AttackLocation"))
        {
            currentPhase = RustyPhase.AttackPhase;
            Invoke(nameof(Jumpscare), 10.0f);
        }

    }
    
    private void BeginMovementAction()
    {
        int movementCheck = Random.Range(0, 20);
        if (currentLocation.CompareTag("AttackLocation") || currentPhase == RustyPhase.AttackPhase)
        {
            Debug.Log("Attacking!!!");
            DisableMovement();
            
        }
        else if (movementCheck <= aILevel)
        {
            IncrementLocation();
            StartNextMovementTimer();
        }
        else
        {
            StartNextMovementTimer();
        }
        


        
    }

    private void Jumpscare()
    {
        Debug.Log("JUMPSCARE!!!!!");
        player.CameraLookBehind(0.5f);
    }

    private float GenerateRandomTime()
    {
        return Random.Range(4.5f, 6.5f);
    }

    private int ChooseRandomLocation(int size)
    {
        return Random.Range(0, size);
    }


    private void IncrementAILevel()
    {
        ++aILevel;
    }
}
