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

    [SerializeField] private int aILevel = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartNextMovementTimer();
        InvokeRepeating(nameof(IncrementAILevel), 20.0f, 20.0f);
        currentPhase = RustyPhase.MovementPhase;
    }

    // Update is called once per frame
    void Update()
    {
        
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
