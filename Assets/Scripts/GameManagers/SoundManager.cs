using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager gameInstance { get; private set; }

    public bool fanActive { get; private set; } = true;

    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private AudioSource fanSource;

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound, Vector3 pos)
    {
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, pos);
            Debug.Log("Sound played successfully");
        }
        Debug.Log("sound not set!");

    }

    public void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            sfx2DSource.PlayOneShot(sound);
            Debug.Log("Sound played successfully");
        }
        Debug.Log("sound not set!");

    }

    public void DisableFan()
    {
        fanSource.enabled = false;
        fanActive = false;
    }

    public void EnableFan()
    {
        fanSource.enabled = true;
        fanActive = true;
    }

}
