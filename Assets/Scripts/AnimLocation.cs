using UnityEngine;

public class AnimLocation : MonoBehaviour
{
    public AnimLocation[] nextLocations;

    [SerializeField] private string animationName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetAnimationName()
    {
        return animationName;
    }
}
