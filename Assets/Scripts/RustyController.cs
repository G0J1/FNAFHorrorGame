using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class RustyController : MonoBehaviour
{
    public enum RustyPhase
    {
        MovementPhase,
        StalkingPhase,
        AttackPhase
    }

    
    [SerializeField] private RustyPhase currentPhase;

    [SerializeField] private AnimLocation startingPos;
    [SerializeField] private AnimLocation currentLocation;
    [SerializeField] private AnimLocation jumpscarePos;
    [SerializeField] private int aILevel = 0;
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animController;

    [SerializeField] private string jumpscareAnim = "Jumpscare";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLocation = startingPos;
        InvokeRepeating(nameof(IncrementAILevel), 20.0f, 20.0f);
        ResetPosition();

        // SetPosition(currentLocation);
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


    public void SetPosition(AnimLocation setPosition)
    {
        gameObject.transform.position = setPosition.transform.position;
        gameObject.transform.rotation = setPosition.transform.rotation;
    }

    public void ResetPosition()
    {
        currentPhase = RustyPhase.MovementPhase;
        currentLocation = startingPos;
        SetPosition(currentLocation);
        Invoke(nameof(StartNextMovementTimer), 10.0f);
    }

    public RustyPhase GetCurrentPhase()
    {
        return currentPhase;
    }

    private void StartNextMovementTimer()
    {
        if (currentPhase == RustyPhase.AttackPhase) { return; }
        float randomTime = GenerateRandomTime();
        Invoke(nameof(BeginMovementAction), randomTime);
    }

    private void DisableMovement()
    {
        CancelInvoke(nameof(BeginMovementAction));
    }



    private void IncrementLocation()
    {
        AnimLocation[] possibleLocations = currentLocation.GetComponent<AnimLocation>().nextLocations;
        AnimLocation nextLocation = currentLocation;

        if (possibleLocations.Length == 1)
        {
            nextLocation = currentLocation.GetComponent<AnimLocation>().nextLocations[0];
        }
        else if (possibleLocations.Length > 1)
        {
            nextLocation = possibleLocations[ChooseRandomLocation(possibleLocations.Length)];
        }

        if (nextLocation != null)
        {
            SetPosition(nextLocation);
            currentLocation = nextLocation;
        }

        if (currentLocation.CompareTag("AttackLocation"))
        {
            currentPhase = RustyPhase.AttackPhase;
            Invoke(nameof(Jumpscare), 10.0f);
        }
        else if (currentLocation.CompareTag("StalkingLocation"))
        {
            currentPhase = RustyPhase.StalkingPhase;
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

    public void Jumpscare()
    {
        CamToggleManager.camToggleManagerInstance.CloseCams();
        Debug.Log("JUMPSCARE!!!!!");
        DisableMovement();
        player.CameraLookBehind(0.5f);
        SetPosition(jumpscarePos);
        animController.Play(jumpscareAnim, 0, 0.0f);
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
