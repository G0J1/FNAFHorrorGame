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
        player.CameraLookBehind(5.0f);
        if (rusty.currentPhase == RustyController.RustyPhase.AttackPhase)
        {
            // taze rsuty and revert him back to stage
            yield return new WaitForSeconds(2.0f);
            rusty.GetTazed();
            yield return new WaitForSeconds(2.0f);
            player.CameraLookFront(0.5f);
        }
        else if (rusty.currentPhase == RustyController.RustyPhase.StalkingPhase)
        {
            yield return new WaitForSeconds(0.5f);
            rusty.Jumpscare();
        }
        else
        {
            // short the breaker if rusty not in office
            // if rusty in office but not close enough, rusty kills you
            yield return new WaitForSeconds(2.0f);
            player.CameraLookFront(0.5f);
        }


        isZapping = false;
    }
}
