using Unity.VisualScripting;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject pivot;

    [SerializeField] private float rotRate;
    public void Interact()
    {
        Debug.Log("Recharging power!");
        PowerManager.gameInstance.AddPower(5.0f);
        pivot.transform.Rotate(0, -rotRate, 0);
    }
}
