using UnityEngine;
using UnityEngine.UIElements;

public class RustyController : MonoBehaviour
{
    public Location currentLocation;

    [SerializeField] private int aILevel = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(BeginMovementAction), GenerateRandomTime(), GenerateRandomTime());
        InvokeRepeating(nameof(IncrementAILevel), 20.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IncrementLocation()
    {
        Location nextLocation = currentLocation.GetComponent<Location>().nextLocations[0];
        if (nextLocation != null)
        {
            gameObject.transform.position = nextLocation.transform.position;
            currentLocation = nextLocation;
        }
        
    }
    
    private void BeginMovementAction()
    {
        int movementCheck = Random.Range(0, 20);
        if (movementCheck <= aILevel)
        {
            IncrementLocation();
        }
    }

    private float GenerateRandomTime()
    {
        return Random.Range(4.5f, 6.5f);
    }

    private void IncrementAILevel()
    {
        ++aILevel;
    }
}
