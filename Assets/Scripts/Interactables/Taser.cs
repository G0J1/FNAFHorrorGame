using System;
using System.Collections;
using UnityEngine;

public class Taser : MonoBehaviour, IInteractable
{
    [SerializeField] private RustyController rusty;
    [SerializeField] private PlayerController player;


    [SerializeField] private Boolean isZapping;
    [SerializeField] private float drainAmount = 30.0f;

    [SerializeField] private GameObject mesh;
    void Start()
    {
        isZapping = false;
        mesh.SetActive(true);
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
        yield return new WaitForSeconds(1.0f);
        mesh.SetActive(false);
        if (rusty.GetCurrentPhase() == RustyController.RustyPhase.AttackPhase)
        {
            // taze rsuty and revert him back to stage
            yield return new WaitForSeconds(2.0f);
            rusty.GetTazed();
            yield return new WaitForSeconds(0.5f);
            player.endTaze();
            mesh.SetActive(true);
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
            yield return new WaitForSeconds(3.0f);
            PowerManager.gameInstance.SetPower(0f);
            player.endTaze();
            yield return new WaitForSeconds(1.5f);
            mesh.SetActive(true);
        }


        isZapping = false;
    }
}
