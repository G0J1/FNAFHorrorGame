using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject crank;
    public void Interact()
    {
        Debug.Log("Recharging power!");
        PowerManager.gameInstance.AddPower(5.0f);
    }
}
