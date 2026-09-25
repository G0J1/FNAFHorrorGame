using System;
using System.Collections;
using UnityEngine;

public class Taser : MonoBehaviour, IInteractable
{
    [SerializeField] private RustyController rusty;
    [SerializeField] private PlayerController player;

    [SerializeField] private Boolean isZapping;
    [SerializeField] private float drainAmount = 30.0f;

    void Start()
    {
        isZapping = false;
    }
    public void Interact()
    {
        if (!isZapping)
        {
            if (PowerManager.gameInstance.GetCurrentPower() > drainAmount)
            {
                Debug.Log("TAZE!!!!");
                isZapping = true;
                StartCoroutine(TazeRoutine());
                PowerManager.gameInstance.DrainPower(drainAmount);
            }
            
        }
    }

    private IEnumerator TazeRoutine()
    {
        player.BeginTaze();
        if (rusty.GetCurrentPhase() == RustyController.RustyPhase.AttackPhase)
        {
            // taze rsuty and revert him back to stage
            yield return new WaitForSeconds(3.0f);
            rusty.GetTazed();
            yield return new WaitForSeconds(0.5f);
            player.endTaze();
        }
        else if (rusty.GetCurrentPhase() == RustyController.RustyPhase.StalkingPhase)
        {
            yield return new WaitForSeconds(0.5f);
            rusty.Jumpscare();
        }
        else
        {
            // short the breaker if rusty not in office
            /*player.CameraLookFront(0.5f);*/
            PowerManager.gameInstance.DrainPower(100);
            yield return new WaitForSeconds(4.0f);
            player.endTaze();
        }


        isZapping = false;
    }
}
