using UnityEngine;
using UnityEngine.UIElements;

public class RustyController : MonoBehaviour
{
    public Location currentLocation;

    [SerializeField] private int aILevel = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(IncrementLocation), 5.0f, 5.0f);  
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
}
